namespace CodingMonkey.Controllers
{
    using System.Collections.Generic;
    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;
    using Microsoft.AspNet.Mvc;
    using CodingMonkey.CodeExecutor;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Data.Entity;

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
                model.HasErrors = false;
                model.Errors = null;
            }
            else
            {
                model.HasErrors = true;
                model.Errors = new List<CompilerErrorViewModel>();

                foreach (var resultError in result)
                {
                    model.Errors.Add(new CompilerErrorViewModel()
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
        public async Task<JsonResult> Execute(int id, [FromBody] CodeEditorViewModel model)
        {
            var exercise = CodingMonkeyContext.Exercises
                                              .Include(e => e.Template)
                                              .Include(e => e.Tests).ThenInclude(t => t.TestInputs)
                                              .Include(e => e.Tests).ThenInclude(t => t.TestOutput)
                                              .SingleOrDefault(e => e.ExerciseId == id);

            if (exercise == null || exercise.Template == null)
            {
                return Json(string.Empty);
            }

            model.TestResults = new List<TestResultViewModel>();

            // Run Tests
            foreach (var test in exercise.Tests)
            {
                // Create View Model for result
                TestResultViewModel testResult = new TestResultViewModel()
                {
                    Description = test.Description,
                    Inputs = new List<TestResultInputViewModel>()
                };

                List<CodingMonkey.CodeExecutor.TestInput> testInputs = new List<CodingMonkey.CodeExecutor.TestInput>();
                foreach (var testInput in test.TestInputs)
                {

                    if (!GetTestInputForCodeExecutor(testInput, testResult, testInputs))
                    {
                        return Json(string.Empty);
                    }
                }

                // Run the code to get test result
                var compiler = new RoslynCompiler();
                object codeOutput = await compiler.Execute(model.Code,
                                                exercise.Template.ClassName,
                                                exercise.Template.MainMethodName,
                                                testInputs);

                testResult.ActualOutput = codeOutput;

                model.TestResults.Add(testResult);

                switch (test.TestOutput.ValueType)
                {
                    case "String":
                        {
                            testResult.ExpectedOutput = test.TestOutput.Value;
                            testResult.TestPassed = testResult.ActualOutput == testResult.ExpectedOutput;
                            break;
                        }
                    case "Integer":
                        {
                            int outputValue;
                            int.TryParse(test.TestOutput.Value, out outputValue);

                            testResult.ExpectedOutput = outputValue;
                            testResult.TestPassed = testResult.ActualOutput == testResult.ExpectedOutput;

                            break;
                        }
                    case "Boolean":
                        {
                            bool outputValue;
                            bool.TryParse(test.TestOutput.Value, out outputValue);

                            testResult.ExpectedOutput = outputValue;
                            testResult.TestPassed = testResult.ActualOutput == testResult.ExpectedOutput;
                            break;
                        }
                    default:
                        {
                            return null;
                        }
                }
            }

            return Json(model);
        }

        private static bool GetTestInputForCodeExecutor(TestInput testInput, TestResultViewModel testResult, List<CodeExecutor.TestInput> testInputs)
        {
            switch (testInput.ValueType)
            {
                case "String":
                    {
                        testResult.Inputs.Add(
                            new TestResultInputViewModel() { ArgumentName = testInput.ArgumentName, Value = testInput.Value });

                        testInputs.Add(
                            new CodeExecutor.TestInput()
                                {
                                    Value = testInput.Value,
                                    ValueType = testInput.ValueType,
                                    ArgumentName = testInput.ArgumentName
                                });
                        return true;
                    }
                case "Integer":
                    {
                        int inputValue;
                        int.TryParse(testInput.Value, out inputValue);

                        testResult.Inputs.Add(
                            new TestResultInputViewModel() { ArgumentName = testInput.ArgumentName, Value = inputValue });

                        testInputs.Add(
                            new CodeExecutor.TestInput()
                                {
                                    Value = inputValue,
                                    ValueType = testInput.ValueType,
                                    ArgumentName = testInput.ArgumentName
                                });
                        return true;
                    }
                case "Boolean":
                    {
                        bool inputValue;
                        bool.TryParse(testInput.Value, out inputValue);

                        testResult.Inputs.Add(
                            new TestResultInputViewModel() { ArgumentName = testInput.ArgumentName, Value = inputValue });

                        testInputs.Add(
                            new CodeExecutor.TestInput()
                                {
                                    Value = inputValue,
                                    ValueType = testInput.ValueType,
                                    ArgumentName = testInput.ArgumentName
                                });
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
    }
}
