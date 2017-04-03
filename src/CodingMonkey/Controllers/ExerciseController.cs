namespace CodingMonkey.Controllers
{
    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;

    using CodingMonkey.Models.Repositories;

    [Route("api/[controller]/[action]")]
    public class ExerciseController : BaseController
    {
        public CodingMonkeyRepositoryContext CodingMonkeyRepositoryContext { get; set; }

        public IMapper Mapper { get; set; }

        public ExerciseController(CodingMonkeyRepositoryContext codingMonkeyRepositoryContext, IMapper mapper)
        {
            this.CodingMonkeyRepositoryContext = codingMonkeyRepositoryContext;
            this.Mapper = mapper;
        }

        [HttpGet]
        public JsonResult List()
        {
            var exercises = this.CodingMonkeyRepositoryContext.ExerciseRepository.All();

            var vm = Mapper.Map<List<ExerciseViewModel>>(exercises);

            return Json(vm);
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Details(int id)
        {
            var exercise = this.CodingMonkeyRepositoryContext.ExerciseRepository.GetById(id);

            JsonResult result = exercise == null ? Json(string.Empty) : Json(Mapper.Map<ExerciseViewModel>(exercise));

            return result;
        }

        [HttpPost]
        [Authorize]
        public JsonResult Create([FromBody] ExerciseViewModel vm)
        {
            if (vm == null) return Json(string.Empty);

            Exercise exerciseToCreate = Mapper.Map<Exercise>(vm);

            try
            {
                if (ModelState.IsValid)
                {
                    exerciseToCreate = this.CodingMonkeyRepositoryContext.ExerciseRepository
                                                                         .Create(exerciseToCreate);
                }
            }
            catch (Exception)
            {
                return DataActionFailedMessage(DataAction.Created);
            }

            vm = Mapper.Map<ExerciseViewModel>(exerciseToCreate);

            return Json(vm);
        }

        [HttpPost]
        [Route("{id}")]
        [Authorize]
        public JsonResult Update(int id, [FromBody] ExerciseViewModel vm)
        {
            if (vm == null) return Json(string.Empty);

            var updatedExercise = Mapper.Map<Exercise>(vm);

            try
            {
                if (ModelState.IsValid)
                {
                    updatedExercise = this.CodingMonkeyRepositoryContext.ExerciseRepository
                                                                        .Update(id, updatedExercise);
                }
            }
            catch (Exception)
            {
                return DataActionFailedMessage(DataAction.Updated);
            }

            vm = Mapper.Map<ExerciseViewModel>(updatedExercise);

            return Json(vm);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            try
            {
                this.CodingMonkeyRepositoryContext.ExerciseRepository.Delete(id);
            }
            catch (Exception)
            {
                return DataActionFailedMessage(DataAction.Deleted);
            }

            return Json(new { deleted = true });
        }
    }
}
