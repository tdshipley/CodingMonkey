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
    public class TestController : BaseController
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
            var tests = CodingMonkeyContext.Tests
                                           .Include(x => x.TestInputs)
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
            var test = CodingMonkeyContext.Tests
                                          .Include(x => x.TestInputs)
                                          .Include(x => x.TestOutput)
                                          .Include(x => x.Exercise)
                                          .SingleOrDefault(e => e.TestId == id && e.Exercise.ExerciseId == exerciseId);

            JsonResult result = test == null ? Json(string.Empty) : Json(Mapper.Map<TestViewModel>(test));

            return result;
        }

        [HttpPost]
        [Authorize]
        public JsonResult Create(int exerciseId, [FromBody] TestViewModel vm)
        {
            if (vm == null) return Json(string.Empty);

            Exercise relatedExercise = CodingMonkeyContext.Exercises
                                                          .SingleOrDefault(x => x.ExerciseId == exerciseId);

            if (relatedExercise == null) return DataActionFailedMessage(DataAction.Created, DataActionFailReason.RecordNotFound);

            Test testToCreate = Mapper.Map<Test>(vm);

            testToCreate.RelateExerciseToTestInMemory(relatedExercise);

            try
            {
                CodingMonkeyContext.Tests.Add(testToCreate);

                if (ModelState.IsValid) CodingMonkeyContext.SaveChanges();

                // Relate Test Inputs & Outputs to newly created test
                testToCreate.RelateTestToTestIoInMemory();

                if (ModelState.IsValid) CodingMonkeyContext.SaveChanges();
            }
            catch (Exception)
            {
                return DataActionFailedMessage(DataAction.Created);
            }

            vm = Mapper.Map<TestViewModel>(testToCreate);

            return Json(vm);
        }

        [HttpPost]
        [Authorize]
        [Route("{id}")]
        public JsonResult Update(int exerciseId, int id, [FromBody] TestViewModel vm)
        {
            if (vm == null) return Json(string.Empty);
            
            var testToUpdate = CodingMonkeyContext.Tests
                                                  .Include(x => x.TestInputs)
                                                  .Include(x => x.TestOutput)
                                                  .Include(x => x.Exercise)
                                                  .SingleOrDefault(e => e.TestId == id && e.Exercise.ExerciseId == exerciseId);

            if (testToUpdate == null) return DataActionFailedMessage(DataAction.Updated, DataActionFailReason.RecordNotFound);

            try
            {
                testToUpdate.Description = vm.Description;

                // Remove deleted test inputs
                var testInputsInDb = CodingMonkeyContext.TestInputs
                                                        .Include(x => x.Test)
                                                        .Where(x => x.Test.TestId == id).ToList();

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

                if (ModelState.IsValid) CodingMonkeyContext.SaveChanges();

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

                    if (ModelState.IsValid) CodingMonkeyContext.SaveChanges();

                    newTestInput.Id = testInputToAdd.TestInputId;
                }
            }
            catch (Exception)
            {
                return DataActionFailedMessage(DataAction.Updated);
            }

            return Json(vm);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            var testToDelete = CodingMonkeyContext.Tests.Include(t => t.TestInputs)
                                                        .Include(t => t.TestOutput)
                                                        .SingleOrDefault(t => t.TestId == id);

            if (testToDelete == null) DataActionFailedMessage(DataAction.Deleted, DataActionFailReason.RecordNotFound);
            
            try
            {
                CodingMonkeyContext.Tests.Remove(testToDelete);
                CodingMonkeyContext.SaveChanges();
            }
            catch (Exception)
            {
                DataActionFailedMessage(DataAction.Deleted);
            }

            return Json(new { deleted = true });
        }
    }
}
