namespace CodingMonkey.Controllers
{
    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    [Route("api/exercise/{exerciseId}/[controller]/[action]")]
    public class TestController : Controller
    {
        public CodingMonkeyContext CodingMonkeyContext { get; set; }

        public IMapper Mapper { get; set; }

        public TestController(CodingMonkeyContext codingMonkeyContext, IMapper mapper)
        {
            this.CodingMonkeyContext = codingMonkeyContext;
            this.Mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public JsonResult List(int exerciseId)
        {
            var tests =
                CodingMonkeyContext.Tests.Include(x => x.TestInputs)
                    .Include(x => x.TestOutput)
                    .Include(x => x.Exercise)
                    .Where(x => x.Exercise.ExerciseId == exerciseId).ToList();

            var vm = this.Mapper.Map<List<TestViewModel>>(tests);

            return Json(vm);
        }

        [HttpGet]
        [Authorize]
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

            var vm = this.Mapper.Map<TestViewModel>(test);

            return Json(vm);
        }

        [HttpPost]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            var result = new Dictionary<string, dynamic>();
            var testToDelete = CodingMonkeyContext.Tests.Include(t => t.TestInputs)
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
