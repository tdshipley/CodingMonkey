namespace CodingMonkey.Controllers
{
    using System;
    using System.Collections.Generic;
    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;
    using Microsoft.AspNet.Mvc;
    using CodingMonkey.CodeExecutor;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Razor.CodeGenerators.Visitors;
    using Microsoft.Data.Entity;

    using Remotion.Linq.Clauses;

    using TestInput = CodingMonkey.Models.TestInput;

    [Route("api/[controller]/[action]/{id}")]
    public class CodeExecutionController : Controller
    {
        [FromServices]
        public CodingMonkeyContext CodingMonkeyContext { get; set; }

        [HttpPost]
        public JsonResult Compile(int id, [FromBody] CodeEditorViewModel model)
        {
            var result = new RoslynCompiler().Compile(model.Code);

            if (result == null || result.Count == 0)
            {
                model.HasCompilerErrors = false;
                model.CompilerErrors = null;
            }
            else
            {
                model.HasCompilerErrors = true;
                model.CompilerErrors = new List<CompilerErrorViewModel>();

                foreach (var resultError in result)
                {
                    model.CompilerErrors.Add(new CompilerErrorViewModel()
                    {
                        Id = resultError.Id,
                        Severity = resultError.Severity,
                        Message = resultError.Message,
                        LineNumberStart = resultError.StartLineNumber,
                        LineNumberEnd = resultError.EndLineNumber,
                        ColStart = resultError.ColStart,
                        ColEnd = resultError.ColEnd,
                        ErrorLength = resultError.ErrorLength
                    });
                }
            }

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

            vm.TestResults = new List<TestResultViewModel>();

            // Run Tests
            foreach (var test in exercise.Tests)
            {
                // Create View Model for result
                TestResultViewModel testResult = new TestResultViewModel()
                {
                    Description = test.Description,
                    Inputs = new List<TestResultInputViewModel>(),
                    TestExecuted = false,
                    TestPassed = false
                };

                var testInputs = new List<CodingMonkey.CodeExecutor.TestInput>();
                if (test.TestInputs.Any(testInput => !GetTestInputForCodeExecutor(testInput, testResult, testInputs)))
                {
                    return this.Json(string.Empty);
                }

                // Run the code to get test result
                var compiler = new RoslynCompiler();
                ExecutionResult executionResult = await compiler.ExecuteAsync(vm.Code,
                                                exercise.Template.ClassName,
                                                exercise.Template.MainMethodName,
                                                testInputs);

                if (!executionResult.Successful)
                {
                    vm.TestResults = null;
                    vm.CompilerErrors = null;
                    vm.HasCompilerErrors = false;
                    vm.HasRuntimeError = true;

                    vm.RuntimeError = new RuntimeErrorViewModel()
                                          {
                                              Message = executionResult.Error.Message,
                                              HelpLink = executionResult.Error.HelpLink
                                          };

                    return Json(vm);
                }

                testResult.ActualOutput = executionResult.Value;

                vm.TestResults.Add(testResult);

                if (AddTestResult(test, testResult)) return null;

                testResult.TestExecuted = true;
            }

            return Json(vm);
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

        private static bool GetTestInputForCodeExecutor(TestInput testInput, TestResultViewModel testResult, List<CodeExecutor.TestInput> testInputs)
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
            List<CodeExecutor.TestInput> testInputs,
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
                new CodeExecutor.TestInput()
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
