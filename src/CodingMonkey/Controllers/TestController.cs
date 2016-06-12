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
    using Models.Repositories;

    [Route("api/exercise/{exerciseId}/[controller]/[action]")]
    public class TestController : BaseController
    {
        public CodingMonkeyContext CodingMonkeyContext { get; set; }

        public CodingMonkeyRepositoryContext CodingMonkeyRepositoryContext { get; set; }

        public IMapper Mapper { get; set; }

        public TestController(CodingMonkeyContext codingMonkeyContext, CodingMonkeyRepositoryContext codingMonkeyRepositoryContext, IMapper mapper)
        {
            this.CodingMonkeyContext = codingMonkeyContext;
            this.CodingMonkeyRepositoryContext = codingMonkeyRepositoryContext;
            this.Mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public JsonResult List(int exerciseId)
        {
            var tests = CodingMonkeyRepositoryContext.TestRepository.All(exerciseId);

            var vm = this.Mapper.Map<List<TestViewModel>>(tests);

            return Json(vm);
        }

        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public JsonResult Details(int exerciseId, int id)
        {
            var test = CodingMonkeyRepositoryContext.TestRepository.GetById(id);

            JsonResult result = test == null ? Json(string.Empty) : Json(Mapper.Map<TestViewModel>(test));

            return result;
        }

        [HttpPost]
        [Authorize]
        public JsonResult Create(int exerciseId, [FromBody] TestViewModel vm)
        {
            if (vm == null) return Json(string.Empty);

            Test testToCreate = Mapper.Map<Test>(vm);

            Test createdTest = CodingMonkeyRepositoryContext.TestRepository.Create(exerciseId, testToCreate);

            vm = Mapper.Map<TestViewModel>(createdTest);

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

            var updatedTest = Mapper.Map<Test>(vm);

            updatedTest = CodingMonkeyRepositoryContext.TestRepository.Update(exerciseId, id, updatedTest);

            return Json(vm);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            CodingMonkeyRepositoryContext.TestRepository.Delete(id);

            return Json(new { deleted = true });
        }
    }
}
