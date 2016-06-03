namespace CodingMonkey.Controllers
{
    using System;
    using System.Collections.Generic;
    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;
    using Microsoft.AspNetCore.Mvc;

    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using CodingMonkey.CodeExecutorModels;
    using CodingMonkey.Configuration;

    using IdentityModel.Client;

    using Microsoft.Extensions.Options;

    using Newtonsoft.Json;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    [Route("api/[controller]/[action]/{id}")]
    public class CodeExecutionController : Controller
    {
        public CodingMonkeyContext CodingMonkeyContext { get; set; }
        
        private IOptions<AppConfig> _appConfig { get; set; }
        private IOptions<IdentityServerConfig> _identityServerConfig { get; set; }

        public CodeExecutionController(IOptions<AppConfig> appConfig, IOptions<IdentityServerConfig> identityServerConfig, CodingMonkeyContext codingMonkeyContext)
        {
            this._appConfig = appConfig;
            this._identityServerConfig = identityServerConfig;
            this.CodingMonkeyContext = codingMonkeyContext;
        }

        [HttpPost]
        public async Task<JsonResult> Compile(int id, [FromBody] CodeEditorViewModel vm)
        {
            CodeSubmission codeToSubmit = new CodeSubmission()
                                              {
                                                  Code = vm.Code
                                              };

            var response = await PostRequestToCodeExecutorAsync("api/compile", codeToSubmit);

            if (response.IsSuccessStatusCode)
            {
                CodeSubmission codeSubmissionCompiled = JsonConvert.DeserializeObject<CodeSubmission>(response.Content.ReadAsStringAsync().Result);

                vm.HasCompilerErrors = codeSubmissionCompiled.ResultSummary.HasCompilerErrors;

                if (!vm.HasCompilerErrors)
                {
                    return Json(vm);
                }

                vm.CompilerErrors = new List<CompilerErrorViewModel>();

                foreach (var compilerError in codeSubmissionCompiled.ResultSummary.CompilerErrors)
                {
                    vm.CompilerErrors.Add(new CompilerErrorViewModel()
                                              {
                                                  ColEnd = compilerError.ColEnd,
                                                  ColStart = compilerError.ColStart,
                                                  ErrorLength = compilerError.ErrorLength,
                                                  Id = compilerError.Id,
                                                  LineNumberEnd = compilerError.EndLineNumber,
                                                  LineNumberStart = compilerError.StartLineNumber,
                                                  Message = compilerError.Message,
                                                  Severity = compilerError.Severity
                                              });
                }
            }
            else
            {
                return Json(string.Empty);
            }

            return Json(vm);
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

            if (!coreTestsPassed)
            {
                return Json(vm);
            }

            var codeToExecute = new CodeSubmission()
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
                                             ExpectedOutput = new CodeTestExpectedOutput()
                                                                  {
                                                                      Value = test.TestOutput.Value,
                                                                      ValueType = test.TestOutput.ValueType
                                                                  },
                                             Inputs = new List<CodeTestInput>()
                                         };

                foreach (var testInput in test.TestInputs)
                {
                    testToRun.Inputs.Add(
                        new CodeTestInput()
                            {
                                ArgumentName = testInput.ArgumentName,
                                Value = testInput.Value,
                                ValueType = testInput.ValueType
                            });
                }

                codeToExecute.Tests.Add(testToRun);
            }

            var response = await PostRequestToCodeExecutorAsync("api/Execution", codeToExecute);

            if (response.IsSuccessStatusCode)
            {
                CodeSubmission codeSubmissionExecuted = JsonConvert.DeserializeObject<CodeSubmission>(response.Content.ReadAsStringAsync().Result);

                if (codeSubmissionExecuted.ResultSummary.HasRuntimeError)
                {
                    vm.HasRuntimeError = codeSubmissionExecuted.ResultSummary.HasRuntimeError;
                    vm.HasCompilerErrors = false;
                    vm.RuntimeError = new RuntimeErrorViewModel()
                                          {
                                              Message = codeSubmissionExecuted.ResultSummary.RuntimeError.Message,
                                              HelpLink = codeSubmissionExecuted.ResultSummary.RuntimeError.HelpLink
                                          };

                    return Json(vm);
                }

                if (codeSubmissionExecuted.ResultSummary.HasCompilerErrors)
                {
                    vm.HasRuntimeError = false;
                    vm.HasCompilerErrors = codeSubmissionExecuted.ResultSummary.HasCompilerErrors;

                    vm.CompilerErrors = new List<CompilerErrorViewModel>();

                    foreach (var compilerError in codeSubmissionExecuted.ResultSummary.CompilerErrors)
                    {
                        vm.CompilerErrors.Add(
                            new CompilerErrorViewModel()
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

                    return Json(vm);
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

        private async Task<HttpResponseMessage> PostRequestToCodeExecutorAsync(string path, object dataToPost)
        {
            var baseAddress = this._appConfig.Value.CodeExecutorApiEndpoint;

            var httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };

            var accessToken = await this.GetCodeExecutorAccessTokenAsync();

            httpClient.SetBearerToken(accessToken);
            return httpClient.PostAsync(
                    path,
                    new StringContent(
                        JsonConvert.SerializeObject(dataToPost),
                        Encoding.UTF8,
                        "application/json")).Result;
        }

        private async Task<string> GetCodeExecutorAccessTokenAsync()
        {
            var tokenClient = new TokenClient(
                this._appConfig.Value.IdentityServerApiEndpoint + "/connect/token",
                this._identityServerConfig.Value.ClientId,
                this._identityServerConfig.Value.ClientSecret);

            var accessTokenRequest = await tokenClient.RequestClientCredentialsAsync("CodingMonkey.CodeExecutor");
            var accessToken = accessTokenRequest.AccessToken;
            return accessToken;
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
                                                                 Inputs = new List<TestResultInputViewModel>()
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
                                                                 Inputs = new List<TestResultInputViewModel>()
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
                                             Description = $"Class '{className}' has method named '{mainMethodName}' with signature of '{mainMethodSignature}'",
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
    }
}
