namespace CodingMonkey.Controllers
{
    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;

    using Microsoft.AspNet.Mvc;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Data.Entity;

    [Route("api/exercise/{exerciseId}/[controller]/[action]")]
    public class TestController : Controller
    {
        [FromServices]
        public CodingMonkeyContext CodingMonkeyContext { get; set; }

        [HttpGet]
        public JsonResult List(int exerciseId)
        {
            var tests =
                CodingMonkeyContext.Tests.Include(x => x.TestInputs)
                    .Include(x => x.TestOutput)
                    .Include(x => x.Exercise)
                    .Where(x => x.Exercise.ExerciseId == exerciseId);

            List<TestViewModel> vm = new List<TestViewModel>();

            foreach (var test in tests)
            {
                List<TestInputViewModel> testInputsViewModel = new List<TestInputViewModel>();

                foreach (var testInput in test.TestInputs)
                {
                    testInputsViewModel.Add(
                        new TestInputViewModel()
                            {
                                Id = testInput.TestInputId,
                                ArgumentName = testInput.ArgumentName,
                                ValueType = testInput.ValueType,
                                Value = testInput.Value
                            });
                }

                vm.Add(
                    new TestViewModel()
                        {
                            Id = test.TestId,
                            Description = test.Description,
                            TestInputs = testInputsViewModel,
                            TestOutput =
                                new TestOutputViewModel()
                                    {
                                        Id = test.TestOutput.TestOutputId,
                                        ValueType = test.TestOutput.ValueType,
                                        Value = test.TestOutput.Value
                                    }
                        });
            }

            return Json(vm);
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Details(int exerciseId, int id)
        {
            var test =
                CodingMonkeyContext.Tests.Include(x => x.TestInputs)
                    .Include(x => x.TestOutput)
                    .Include(x => x.Exercise)
                    .SingleOrDefault(e => e.TestId == id && e.Exercise.ExerciseId == exerciseId);

            if (test == null)
            {
                return Json(string.Empty);
            }

            List<TestInputViewModel> testInputsViewModel = new List<TestInputViewModel>();

            foreach (var testInput in test.TestInputs)
            {
                testInputsViewModel.Add(
                    new TestInputViewModel()
                        {
                            Id = testInput.TestInputId,
                            ArgumentName = testInput.ArgumentName,
                            ValueType = testInput.ValueType,
                            Value = testInput.Value
                        });
            }

            var vm = new TestViewModel()
                         {
                             Id = test.TestId,
                             Description = test.Description,
                             TestInputs = testInputsViewModel,
                             TestOutput =
                                 new TestOutputViewModel()
                                     {
                                         Id = test.TestOutput.TestOutputId,
                                         ValueType = test.TestOutput.ValueType,
                                         Value = test.TestOutput.Value
                                     }
                         };

            return Json(vm);
        }

        [HttpPost]
        public JsonResult Create(int exerciseId, [FromBody] TestViewModel vm)
        {
            var exceptionResult = new Dictionary<string, dynamic>();

            if (vm == null)
            {
                return Json(string.Empty);
            }

            List<TestInput> testInputsForModel = ConvertTestInputsViewModelToTestInputsModel(vm.TestInputs);
            TestOutput testOutputForModel = ConvertTestOutputViewModelToTestOutputModel(vm.TestOutput);

            Exercise exerciseTestBelongsTo =
                CodingMonkeyContext.Exercises.SingleOrDefault(x => x.ExerciseId == exerciseId);

            Test testToCreate = new Test()
                                    {
                                        Description = vm.Description,
                                        TestInputs = testInputsForModel,
                                        TestOutput = testOutputForModel,
                                        Exercise = exerciseTestBelongsTo
                                    };

            try
            {
                CodingMonkeyContext.Tests.Add(testToCreate);

                if (ModelState.IsValid)
                {
                    CodingMonkeyContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                exceptionResult["created"] = false;
                exceptionResult["reason"] = "exception thrown";

                return Json(exceptionResult);
            }

            // Create view model with data from db
            vm.Id = testToCreate.TestId;

            vm.TestInputs.Clear();
            // Relate created test inputs
            foreach (var testInput in testInputsForModel)
            {
                testInput.Test = testToCreate;
                vm.TestInputs.Add(
                    new TestInputViewModel()
                        {
                            Id = testInput.TestInputId,
                            ArgumentName = testInput.ArgumentName,
                            ValueType = testInput.ValueType,
                            Value = testInput.Value
                        });
            }

            // Relate created test output
            testOutputForModel.TestForeignKey = testToCreate.TestId;
            testOutputForModel.Test = testToCreate;
            vm.TestOutput = new TestOutputViewModel()
                                {
                                    Id = testOutputForModel.TestOutputId,
                                    Value = testOutputForModel.Value,
                                    ValueType = testOutputForModel.ValueType
                                };

            try
            {
                if (ModelState.IsValid)
                {
                    CodingMonkeyContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "exception thrown";

                return Json(exceptionResult);
            }

            return Json(vm);
        }

        [HttpPost]
        [Route("{id}")]
        public JsonResult Update(int exerciseId, int id, [FromBody] TestViewModel vm)
        {
            if (vm == null)
            {
                return Json(string.Empty);
            }

            var exceptionResult = new Dictionary<string, dynamic>();
            var testToUpdate =
                CodingMonkeyContext.Tests.Include(x => x.TestInputs)
                    .Include(x => x.TestOutput)
                    .Include(x => x.Exercise)
                    .SingleOrDefault(e => e.TestId == id && e.Exercise.ExerciseId == exerciseId);

            if (testToUpdate == null)
            {
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "record not found";

                return Json(exceptionResult);
            }

            try
            {
                testToUpdate.Description = vm.Description;

                // Remove deleted test inputs
                var testInputsInDb =
                    CodingMonkeyContext.TestInputs.Include(x => x.Test).Where(x => x.Test.TestId == id).ToList();

                foreach (var testInput in testInputsInDb)
                {
                    if (!vm.TestInputs.Any(x => x.Id == testInput.TestInputId))
                    {
                        CodingMonkeyContext.TestInputs.Remove(testInput);
                        testToUpdate.TestInputs.Remove(testInput);
                    }
                }

                // Update existing test inputs
                foreach (var testInputModel in testToUpdate.TestInputs)
                {
                    // Update test input
                    var updatedTestInput = vm.TestInputs.FirstOrDefault(x => x.Id == testInputModel.TestInputId);

                    // Create new test input
                    if (updatedTestInput != null)
                    {
                        testInputModel.ArgumentName = updatedTestInput.ArgumentName;
                        testInputModel.Value = updatedTestInput.Value;
                        testInputModel.ValueType = updatedTestInput.ValueType;
                    }
                }

                // Update test outputs
                testToUpdate.TestOutput.Value = vm.TestOutput.Value;
                testToUpdate.TestOutput.ValueType = vm.TestOutput.ValueType;

                if (ModelState.IsValid)
                {
                    CodingMonkeyContext.SaveChanges();
                }

                // Create new test inputs
                foreach (var newTestInput in vm.TestInputs.Where(x => x.Id == null))
                {
                    TestInput testInputToAdd = new TestInput()
                                                   {
                                                       ArgumentName = newTestInput.ArgumentName,
                                                       Value = newTestInput.Value,
                                                       ValueType = newTestInput.ValueType
                                                   };

                    testToUpdate.TestInputs.Add(testInputToAdd);

                    if (ModelState.IsValid)
                    {
                        CodingMonkeyContext.SaveChanges();
                    }

                    newTestInput.Id = testInputToAdd.TestInputId;
                }
            }
            catch (Exception)
            {
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "exception thrown";

                return Json(exceptionResult);
            }

            return Json(vm);
        }

        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            var result = new Dictionary<string, dynamic>();
            var testToDelete =
                CodingMonkeyContext.Tests.Include(t => t.TestInputs)
                    .Include(t => t.TestOutput)
                    .SingleOrDefault(t => t.TestId == id);

            if (testToDelete == null)
            {
                result["deleted"] = false;
                result["reason"] = "record not found";
            }
            else
            {
                try
                {
                    // Remove test inputs
                    foreach (var testInput in testToDelete.TestInputs)
                    {
                        CodingMonkeyContext.TestInputs.Remove(testInput);
                    }

                    // Remove test output
                    CodingMonkeyContext.TestOutputs.Remove(testToDelete.TestOutput);

                    CodingMonkeyContext.Tests.Remove(testToDelete);
                    CodingMonkeyContext.SaveChanges();
                    result["deleted"] = true;
                }
                catch (Exception)
                {
                    result["deleted"] = false;
                    result["reason"] = "exception thrown";
                }
            }

            return Json(result);
        }

        private List<TestInput> ConvertTestInputsViewModelToTestInputsModel(List<TestInputViewModel> testInputViewModel)
        {
            List<TestInput> testInputs = new List<TestInput>();

            foreach (var testInput in testInputViewModel)
            {
                testInputs.Add(
                    new TestInput()
                        {
                            Value = testInput.Value,
                            ValueType = testInput.ValueType,
                            ArgumentName = testInput.ArgumentName
                        });
            }

            return testInputs;
        }

        private TestOutput ConvertTestOutputViewModelToTestOutputModel(TestOutputViewModel testOutputViewModel)
        {
            return new TestOutput() { Value = testOutputViewModel.Value, ValueType = testOutputViewModel.ValueType };
        }
    }
}
