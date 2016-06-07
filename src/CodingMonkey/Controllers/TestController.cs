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
            
            var existingTest = CodingMonkeyContext.Tests
                                                  .Include(x => x.TestInputs)
                                                  .Include(x => x.TestOutput)
                                                  .Include(x => x.Exercise)
                                                  .SingleOrDefault(e => e.TestId == id && e.Exercise.ExerciseId == exerciseId);

            if (existingTest == null) return DataActionFailedMessage(DataAction.Updated, DataActionFailReason.RecordNotFound);

            try
            {
                //TODO: When EF7 Introduces AddOrUpdate - use that instead of code below
                // http://stackoverflow.com/questions/36208580/what-happened-to-addorupdate-in-ef-7
                var updatedTest = Mapper.Map<Test>(vm);

                CodingMonkeyContext.TestOutputs.Remove(existingTest.TestOutput);
                CodingMonkeyContext.TestInputs.RemoveRange(existingTest.TestInputs);

                if (ModelState.IsValid) CodingMonkeyContext.SaveChanges();

                existingTest.Description = updatedTest.Description;
                existingTest.TestOutput = updatedTest.TestOutput;
                existingTest.TestInputs = updatedTest.TestInputs;

                // Ensure any test input / outputs are related in case new ones added
                existingTest.RelateTestToTestIoInMemory();

                if (ModelState.IsValid) CodingMonkeyContext.SaveChanges();
            }
            catch (Exception ex)
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
                if (ModelState.IsValid) CodingMonkeyContext.SaveChanges();
            }
            catch (Exception)
            {
                DataActionFailedMessage(DataAction.Deleted);
            }

            return Json(new { deleted = true });
        }
    }
}
