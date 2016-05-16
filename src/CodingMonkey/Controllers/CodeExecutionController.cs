namespace CodingMonkey.Controllers
{
    using System;
    using System.Collections.Generic;
    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;
    using Microsoft.AspNet.Mvc;
    using CodingMonkey.CodeExecutor;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using CodingMonkey.CodeExecutorModels;
    using CodingMonkey.Configuration;
    using CodingMonkey.Structs;

    using Microsoft.Data.Entity;
    using Microsoft.Extensions.OptionsModel;
    using IdentityModel.Client;

    using Microsoft.AspNet.Server.Kestrel.Filter;

    using Newtonsoft.Json;

    using TestInput = CodingMonkey.Models.TestInput;

    [Route("api/[controller]/[action]/{id}")]
    public class CodeExecutionController : Controller
    {
        [FromServices]
        public CodingMonkeyContext CodingMonkeyContext { get; set; }
        
        private IOptions<AppConfig> _appConfig { get; set; }
        private IOptions<IdentityServerConfig> _identityServerConfig { get; set; }

        public CodeExecutionController(IOptions<AppConfig> appConfig, IOptions<IdentityServerConfig> identityServerConfig)
        {
            _appConfig = appConfig;
            _identityServerConfig = identityServerConfig;
        }

        [HttpPost]
        public JsonResult Compile(int id, [FromBody] CodeEditorViewModel model)
        {
            object result = null; // new RoslynCompiler().Compile(model.Code);

            if (result == null)
            {
                model.HasCompilerErrors = false;
                model.CompilerErrors = null;
            }
            //else
            //{
            //    model.HasCompilerErrors = true;
            //    model.CompilerErrors = new List<CompilerErrorViewModel>();

            //    foreach (var resultError in result)
            //    {
            //        model.CompilerErrors.Add(new CompilerErrorViewModel()
            //        {
            //            Id = resultError.Id,
            //            Severity = resultError.Severity,
            //            Message = resultError.Message,
            //            LineNumberStart = resultError.StartLineNumber,
            //            LineNumberEnd = resultError.EndLineNumber,
            //            ColStart = resultError.ColStart,
            //            ColEnd = resultError.ColEnd,
            //            ErrorLength = resultError.ErrorLength
            //        });
            //    }
            //}

            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> Execute(int id, [FromBody] CodeEditorViewModel vm)
        {
            var exercise = CodingMonkeyContext.Exercises
                                              .Include(e => e.Template)
                                              .Include(e => e.Tests).ThenInclude(t => t.TestInputs)
                                              .Include(e => e.Tests).ThenInclude(t => t.TestOutput)
                                              .SingleOrDefault(e => e.ExerciseId == id);

            if (exercise?.Template == null)
            {
                return Json(string.Empty);
            }

            vm.AllTestsExecuted = true;
            vm.TestResults = new List<TestResultViewModel>();

            bool coreTestsPassed = RunCoreTests(vm.Code, exercise.Template.ClassName, exercise.Template.MainMethodName, exercise.Template.MainMethodSignature, vm.TestResults);

            // Run the code to get test result
            var tokenClient = new TokenClient(
                                this._appConfig.Value.IdentityServerApiEndpoint + "/connect/token",
                                this._identityServerConfig.Value.ClientId,
                                this._identityServerConfig.Value.ClientSecret);

            var accessTokenRequest = await tokenClient.RequestClientCredentialsAsync("CodingMonkey.CodeExecutor");
            var accessToken = accessTokenRequest.AccessToken;

            var baseAddress = this._appConfig.Value.CodeExecutorApiEndpoint;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            CodeSubmission codeToExecute = new CodeSubmission()
            {
                Code = vm.Code,
                CodeTemplate = new CodeTemplate()
                                {
                                    ClassName = exercise.Template.ClassName,
                                    MainMethodName = exercise.Template.MainMethodName
                                },
                Tests = new List<CodeTest>()
            };

            foreach (var test in exercise.Tests)
            {
                CodeTest testToRun = new CodeTest()
                                         {
                                             Description = test.Description,
                                             ExpectedOutput =
                                                 new CodeTestExpectedOutput()
                                                     {
                                                         Value = test.TestOutput.Value,
                                                         ValueType =
                                                             test.TestOutput.ValueType
                                                     },
                                             Inputs = new List<CodeTestInput>()
                                         };

                foreach (var testInput in test.TestInputs)
                {
                    testToRun.Inputs.Add(new CodeTestInput()
                                             {
                                                 ArgumentName = testInput.ArgumentName,
                                                 Value = testInput.Value,
                                                 ValueType = testInput.ValueType
                                             });
                }

                codeToExecute.Tests.Add(testToRun);
            }

            httpClient.SetBearerToken(accessToken);
            var response = httpClient.PostAsync("api/Execution", new StringContent(
                                                JsonConvert.SerializeObject(codeToExecute).ToString(), // Might need to use convert to JSON here
                                                Encoding.UTF8,
                                                "application/json")).Result;

            if (response.IsSuccessStatusCode)
            {
                CodeSubmission codeSubmissionExecuted =
                    JsonConvert.DeserializeObject<CodeSubmission>(response.Content.ReadAsStringAsync().Result);

                if (codeSubmissionExecuted.ResultSummary.HasRuntimeError)
                {
                    vm.HasRuntimeError = codeSubmissionExecuted.ResultSummary.HasRuntimeError;
                    vm.HasCompilerErrors = false;
                    vm.RuntimeError = new RuntimeErrorViewModel()
                                          {
                                              Message = codeSubmissionExecuted.ResultSummary
                                                                              .RuntimeError
                                                                              .Message,

                                              HelpLink = codeSubmissionExecuted.ResultSummary
                                                                               .RuntimeError
                                                                               .HelpLink
                                          };

                    return this.Json(vm);
                }

                if (codeSubmissionExecuted.ResultSummary.HasCompilerErrors)
                {
                    vm.HasRuntimeError = false;
                    vm.HasCompilerErrors = codeSubmissionExecuted.ResultSummary.HasCompilerErrors;

                    vm.CompilerErrors = new List<CompilerErrorViewModel>();

                    foreach (var compilerError in codeSubmissionExecuted.ResultSummary.CompilerErrors)
                    {
                        vm.CompilerErrors.Add(new CompilerErrorViewModel()
                        {
                            ColEnd = compilerError.ColEnd,
                            ColStart = compilerError.ColEnd,
                            ErrorLength = compilerError.ErrorLength,
                            Id = compilerError.Id,
                            LineNumberEnd = compilerError.EndLineNumber,
                            LineNumberStart = compilerError.StartLineNumber,
                            Message = compilerError.Message,
                            Severity = compilerError.Severity
                        });
                    }

                    return this.Json(vm);
                }

                vm.AllTestsExecuted = codeSubmissionExecuted.ResultSummary.AllTestsExecuted;

                foreach (var test in codeSubmissionExecuted.Tests)
                {
                    var testResult = new TestResultViewModel()
                                         {
                                             ActualOutput = test.ActualOutput,
                                             Description = test.Description,
                                             ExpectedOutput = test.ExpectedOutput.Value,
                                             Inputs = new List<TestResultInputViewModel>(),
                                             TestExecuted = test.Result.TestExecuted,
                                             TestPassed = test.Result.TestPassed
                                         };

                    vm.TestResults.Add(testResult);

                    foreach (var testInput in test.Inputs)
                    {
                        testResult.Inputs.Add(
                            new TestResultInputViewModel()
                                {
                                    ArgumentName = testInput.ArgumentName,
                                    Value = testInput.Value
                                });
                    }
                }
            }
            else
            {
                return Json(string.Empty);
            }

            return Json(vm);
        }

        private static bool RunCoreTests(string code, string className, string mainMethodName, string mainMethodSignature, List<TestResultViewModel> testResults)
        {
            bool coreTestsPassed = true;

            // Check code has a public class matching expected
            string classPattern = $"public\\s*class\\s*{className}(\\n*|\\s*){{";
            var containsPublicClass = new TestResultViewModel()
                                          {
                                              Description =
                                                  $"Code contains a Public Class named '{className}'",
                                              ExpectedOutput = true,
                                              ActualOutput = true,
                                              TestPassed = true,
                                              TestExecuted = true,
                                              Inputs = new List<TestResultInputViewModel>()
                                          };

            if (!Regex.Match(code, classPattern).Success)
            {
                coreTestsPassed = false;
                containsPublicClass.TestPassed = false;
                containsPublicClass.ActualOutput = false;
            }

            // Check code does not have private, protected, internal constructor
            string constructorPattern = $"(private|internal|protected|\\s*)\\s*{className}\\s*\\(";
            string publicConstructorPattern = $"public\\s*{className}\\s*\\(";
            var doesNotConatainPublicClassConstructor = new TestResultViewModel()
                                                             {
                                                                 Description =
                                                                     $"Code does not contain a Internal, Private or Protected Class Constructor '{className}'.",
                                                                 ExpectedOutput = true,
                                                                 ActualOutput = true,
                                                                 TestPassed = true,
                                                                 TestExecuted = true,
                                                                 Inputs =
                                                                     new List<TestResultInputViewModel>()
                                                             };

            bool publicConstructorFound = Regex.Match(code, publicConstructorPattern).Success;
            if (Regex.Match(code, constructorPattern).Success && !publicConstructorFound)
            {
                coreTestsPassed = false;
                doesNotConatainPublicClassConstructor.TestPassed = false;
                doesNotConatainPublicClassConstructor.ActualOutput = false;
            }

            // If code has public constructor check it does not take any arguments
            var publicClassConstructorTakesNoArguments = new TestResultViewModel()
                                                             {
                                                                 Description =
                                                                     $"Public Constructor in Class '{className}' takes no arguments.",
                                                                 ExpectedOutput = true,
                                                                 ActualOutput = true,
                                                                 TestPassed = true,
                                                                 TestExecuted = true,
                                                                 Inputs =
                                                                     new List
                                                                     <TestResultInputViewModel>()
                                                             };

            if (publicConstructorFound)
            {
                string publicConstructorWithNoArgumentsPattern = publicConstructorPattern + "\\s*\\)";

                if (!Regex.Match(code, publicConstructorWithNoArgumentsPattern).Success)
                {
                    coreTestsPassed = false;
                    publicClassConstructorTakesNoArguments.TestPassed = false;
                    publicClassConstructorTakesNoArguments.ActualOutput = false;
                }
            }

            // Check code has main method
            string mainMethodSignaturePattern = mainMethodSignature.Replace(" ", "\\s*")
                                                          .Replace("(", "\\(")
                                                          .Replace(")", "\\)")
                                                          .Replace(",", "\\s*,");

            var containsMainMethod = new TestResultViewModel()
                                         {
                                             Description =
                                                 $"Class '{className}' has method named '{mainMethodName}' with signature of '{mainMethodSignature}'",
                                             ExpectedOutput = true,
                                             ActualOutput = true,
                                             TestPassed = true,
                                             TestExecuted = true,
                                             Inputs = new List<TestResultInputViewModel>()
                                         };

            if (!Regex.Match(code, mainMethodSignaturePattern).Success)
            {
                coreTestsPassed = false;
                containsMainMethod.TestPassed = false;
                containsMainMethod.ActualOutput = false;
            }

            testResults.AddRange(new List<TestResultViewModel>()
                                     {
                                         containsPublicClass,
                                         doesNotConatainPublicClassConstructor,
                                         containsMainMethod
                                     });

            if (publicConstructorFound)
            {
                testResults.Add(publicClassConstructorTakesNoArguments);
            }

            return coreTestsPassed;
        }

        private static bool AddTestResult(Test test, TestResultViewModel testResult)
        {
            switch (test.TestOutput.ValueType)
            {
                case "String":
                    {
                        AddResultToTestResult<string>(testResult, test.TestOutput.Value);
                        break;
                    }
                case "Integer":
                    {
                        AddResultToTestResult<int>(testResult, test.TestOutput.Value);
                        break;
                    }
                case "Boolean":
                    {
                        AddResultToTestResult<bool>(testResult, test.TestOutput.Value);
                        break;
                    }
                default:
                    {
                        return true;
                    }
            }
            return false;
        }

        private static bool GetTestInputForCodeExecutor(TestInput testInput, TestResultViewModel testResult, List<ExecutionTestInput> testInputs)
        {
            switch (testInput.ValueType)
            {
                case "String":
                    {
                        return AddTestInputForCodeExecutorAndResult<string>(testInputs, testResult, testInput);
                    }
                case "Integer":
                    {
                        return AddTestInputForCodeExecutorAndResult<int>(testInputs, testResult, testInput);
                    }
                case "Boolean":
                    {
                        return AddTestInputForCodeExecutorAndResult<bool>(testInputs, testResult, testInput);
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        private static bool AddTestInputForCodeExecutorAndResult<T>(
            List<ExecutionTestInput> testInputs,
            TestResultViewModel testResult,
            TestInput testInputToAdd)
        {
            var valueToAdd = GetValue<T>(testInputToAdd.Value);

            if (valueToAdd == null)
            {
                return false;
            }

            testResult.Inputs.Add(new TestResultInputViewModel() { ArgumentName = testInputToAdd.ArgumentName, Value = (T)valueToAdd });

            testInputs.Add(
                new ExecutionTestInput()
                {
                    Value = (T)valueToAdd,
                    ValueType = testInputToAdd.ValueType,
                    ArgumentName = testInputToAdd.ArgumentName
                });

            return true;
        }

        private static void AddResultToTestResult<T>(TestResultViewModel testResult, object expectedOutputValue)
        {
            var value = GetValue<T>(expectedOutputValue);

            testResult.ExpectedOutput = (T)value;
            testResult.TestPassed = testResult.ActualOutput.Equals((T)testResult.ExpectedOutput);
        }

        private static object GetValue<T>(object value)
        {
            switch (typeof(T).ToString())
            {
                case "System.String":
                    {
                        return value.ToString();
                    }
                case "System.Int32":
                    {
                        int inputValue;
                        bool success = int.TryParse(value.ToString(), out inputValue);

                        if (success)
                        {
                            return inputValue;
                        }

                        return null;
                    }
                case "System.Boolean":
                    {
                        bool inputValue;
                        bool success = bool.TryParse(value.ToString(), out inputValue);

                        if (success)
                        {
                            return inputValue;
                        }

                        return null;
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
