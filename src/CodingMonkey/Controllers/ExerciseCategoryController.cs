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

    [Route("api/[controller]/[action]")]
    public class ExerciseCategoryController : BaseController
    {
        public CodingMonkeyContext CodingMonkeyContext { get; set; }

        public IMapper Mapper { get; set; }

        public ExerciseCategoryController(CodingMonkeyContext codingMonkeyContext, IMapper mapper)
        {
            this.CodingMonkeyContext = codingMonkeyContext;
            this.Mapper = mapper;
        }

        [HttpGet]
        public JsonResult List()
        {
            var exerciseCategories = CodingMonkeyContext.ExerciseCategories
                                                        .Include(e => e.ExerciseExerciseCategories);

            var vm = this.Mapper.Map<List<ExerciseCategoryViewModel>>(exerciseCategories);

            return Json(vm);
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Details(int id)
        {
            var exerciseCategory = CodingMonkeyContext.ExerciseCategories
                                                      .Include(e => e.ExerciseExerciseCategories)
                                                      .SingleOrDefault(e => e.ExerciseCategoryId == id);

            JsonResult result = exerciseCategory == null
                                    ? Json(string.Empty)
                                    : Json(this.Mapper.Map<ExerciseCategoryViewModel>(exerciseCategory));

            return result;
        }

        [HttpPost]
        [Authorize]
        public JsonResult Create([FromBody] ExerciseCategoryViewModel vm)
        {
            if (vm == null)
            {
                return Json(string.Empty);
            }

            ExerciseCategory exerciseCategoryToCreate = new ExerciseCategory()
                                                            {
                                                                Name = vm.Name,
                                                                Description = vm.Description
                                                            };

            try
            {
                CodingMonkeyContext.ExerciseCategories.Add(exerciseCategoryToCreate);

                if (ModelState.IsValid)
                {
                    CodingMonkeyContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                var exceptionResult = new Dictionary<string, dynamic>();

                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "exception thrown";

                return Json(exceptionResult);
            }

            vm = this.Mapper.Map<ExerciseCategoryViewModel>(exerciseCategoryToCreate);

            return Json(vm);
        }

        [HttpPost]
        [Route("{id}")]
        [Authorize]
        public JsonResult Update(int id, [FromBody] ExerciseCategoryViewModel vm)
        {
            if (vm == null)
            {
                return Json(string.Empty);
            }

            var exceptionResult = new Dictionary<string, dynamic>();
            var exerciseCategoryToUpdate = CodingMonkeyContext.ExerciseCategories
                                                              .Include(x => x.ExerciseExerciseCategories)
                                                              .SingleOrDefault(e => e.ExerciseCategoryId == id);

            if (exerciseCategoryToUpdate == null)
            {
                return this.ResultNotFoundMessage(ControllerDataActions.Updated);
            }

            try
            {
                exerciseCategoryToUpdate.Name = vm.Name;
                exerciseCategoryToUpdate.Description = vm.Description;

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

            vm = this.Mapper.Map<ExerciseCategoryViewModel>(exerciseCategoryToUpdate);

            return Json(vm);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public JsonResult Delete(int id)
        {
            var result = new Dictionary<string, dynamic>();
            var exerciseCategoryToDelete =
                CodingMonkeyContext.ExerciseCategories.Include(ec => ec.ExerciseExerciseCategories)
                    .SingleOrDefault(e => e.ExerciseCategoryId == id);

            if (exerciseCategoryToDelete == null)
            {
                return this.ResultNotFoundMessage(ControllerDataActions.Deleted);
            }

            try
            {
                this.CodingMonkeyContext.ExerciseCategories.Remove(exerciseCategoryToDelete);
                this.CodingMonkeyContext.SaveChanges();
                result["deleted"] = true;
            }
            catch (Exception)
            {
                result["deleted"] = false;
                result["reason"] = "exception thrown";
            }

            return Json(result);
        }
    }
}