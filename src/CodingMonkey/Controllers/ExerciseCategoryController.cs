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
    [Route("api/[controller]/[action]")]
    public class ExerciseCategoryController : Controller
    {
        public CodingMonkeyContext CodingMonkeyContext { get; set; }

        public ExerciseCategoryController(CodingMonkeyContext codingMonkeyContext)
        {
            this.CodingMonkeyContext = codingMonkeyContext;
        }

        [HttpGet]
        public JsonResult List()
        {
            var exerciseCategories = CodingMonkeyContext.ExerciseCategories.Include(e => e.ExerciseExerciseCategories);

            List<ExerciseCategoryViewModel> vm = new List<ExerciseCategoryViewModel>();

            foreach (var exerciseCategory in exerciseCategories)
            {
                List<int> exerciseIdsInCategory = GetExercisesInCategory(exerciseCategory.ExerciseCategoryId);

                vm.Add(
                    new ExerciseCategoryViewModel()
                        {
                            Id = exerciseCategory.ExerciseCategoryId,
                            Name = exerciseCategory.Name,
                            Description = exerciseCategory.Description,
                            ExerciseIds = exerciseIdsInCategory
                        });
            }

            return Json(vm);
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Details(int id)
        {
            var exerciseCategory =
                CodingMonkeyContext.ExerciseCategories.SingleOrDefault(e => e.ExerciseCategoryId == id);

            if (exerciseCategory == null)
            {
                return Json(string.Empty);
            }

            List<int> exercisesInCategory = GetExercisesInCategory(exerciseCategory.ExerciseCategoryId);

            var vm = new ExerciseCategoryViewModel()
                             {
                                 Id = exerciseCategory.ExerciseCategoryId,
                                 Name = exerciseCategory.Name,
                                 Description = exerciseCategory.Description,
                                 ExerciseIds = exercisesInCategory
                             };

            return Json(vm);
        }

        [HttpPost]
        [Authorize]
        public JsonResult Create([FromBody] ExerciseCategoryViewModel vm)
        {
            var exceptionResult = new Dictionary<string, dynamic>();

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
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "exception thrown";

                return Json(exceptionResult);
            }

            List<int> exercisesInCategory = GetExercisesInCategory(exerciseCategoryToCreate.ExerciseCategoryId);

            vm.Id = exerciseCategoryToCreate.ExerciseCategoryId;
            vm.Name = exerciseCategoryToCreate.Name;
            vm.Description = exerciseCategoryToCreate.Description;
            vm.ExerciseIds = exercisesInCategory;

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
            var exerciseCategoryToUpdate =
                CodingMonkeyContext.ExerciseCategories.SingleOrDefault(e => e.ExerciseCategoryId == id);

            if (exerciseCategoryToUpdate == null)
            {
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "record not found";

                return Json(exceptionResult);
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

            List<int> exercisesInCategory = GetExercisesInCategory(exerciseCategoryToUpdate.ExerciseCategoryId);

            vm.Id = exerciseCategoryToUpdate.ExerciseCategoryId;
            vm.Name = exerciseCategoryToUpdate.Name;
            vm.Description = exerciseCategoryToUpdate.Description;
            vm.ExerciseIds = exercisesInCategory;

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
                result["deleted"] = false;
                result["reason"] = "record not found";
            }
            else
            {
                try
                {
                    CodingMonkeyContext.ExerciseCategories.Remove(exerciseCategoryToDelete);
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

        private List<int> GetExercisesInCategory(int exerciseCategoryId)
        {
            return
                CodingMonkeyContext.Exercises.Include(e => e.ExerciseExerciseCategories)
                    .Where(e => e.ExerciseExerciseCategories.Any(c => c.ExerciseCategoryId == exerciseCategoryId))
                    .Select(e => e.ExerciseId)
                    .ToList();
        }
    }
}