namespace CodingMonkey.Controllers
{
    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;

    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using AutoMapper;
    using Models.Repositories;

    [Route("api/[controller]/[action]")]
    public class ExerciseCategoryController : BaseController
    {
        public CodingMonkeyRepositoryContext CodingMonkeyRepositoryContext { get; set; }

        public IMapper Mapper { get; set; }

        public ExerciseCategoryController(CodingMonkeyRepositoryContext codingMonkeyRepositoryContext, IMapper mapper)
        {
            this.CodingMonkeyRepositoryContext = codingMonkeyRepositoryContext;
            this.Mapper = mapper;
        }

        [HttpGet]
        public JsonResult List()
        {
            var exerciseCategories = CodingMonkeyRepositoryContext.ExerciseCatgeoryRepository.All();

            var vm = Mapper.Map<List<ExerciseCategoryViewModel>>(exerciseCategories);

            return Json(vm);
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Details(int id)
        {
            var exerciseCategory = CodingMonkeyRepositoryContext.ExerciseCatgeoryRepository.GetById(id);

            JsonResult result = exerciseCategory == null
                                    ? Json(string.Empty)
                                    : Json(Mapper.Map<ExerciseCategoryViewModel>(exerciseCategory));

            return result;
        }

        [HttpPost]
        [Authorize]
        public JsonResult Create([FromBody] ExerciseCategoryViewModel vm)
        {
            if (vm == null)  return Json(string.Empty);

            var exerciseCategoryToCreate = Mapper.Map<ExerciseCategory>(vm);

            try
            {
                if (ModelState.IsValid)
                {
                    exerciseCategoryToCreate = CodingMonkeyRepositoryContext.ExerciseCatgeoryRepository
                                                                            .Create(exerciseCategoryToCreate);
                }
            }
            catch (Exception)
            {
                return Json(this.DataActionFailedMessage(DataAction.Created));
            }

            vm = Mapper.Map<ExerciseCategoryViewModel>(exerciseCategoryToCreate);

            return Json(vm);
        }

        [HttpPost]
        [Route("{id}")]
        [Authorize]
        public JsonResult Update(int id, [FromBody] ExerciseCategoryViewModel vm)
        {
            if (vm == null) return Json(string.Empty);

            ExerciseCategory newExerciseCategory = Mapper.Map<ExerciseCategory>(vm);

            try
            {
                if (ModelState.IsValid)
                {
                    newExerciseCategory = CodingMonkeyRepositoryContext.ExerciseCatgeoryRepository
                                                                       .Update(id, newExerciseCategory);
                }
            }
            catch (Exception)
            {
                return DataActionFailedMessage(DataAction.Updated);
            }

            vm = Mapper.Map<ExerciseCategoryViewModel>(newExerciseCategory);

            return Json(vm);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public JsonResult Delete(int id)
        {
            try
            {
                CodingMonkeyRepositoryContext.ExerciseCatgeoryRepository.Delete(id);
            }
            catch (Exception)
            {
                return DataActionFailedMessage(DataAction.Deleted);
            }

            return Json(new { deleted = true });
        }
    }
}