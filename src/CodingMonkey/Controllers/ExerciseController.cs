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

    [Route("api/[controller]/[action]")]
    public class ExerciseController : Controller
    {
        public CodingMonkeyContext CodingMonkeyContext { get; set; }

        public ExerciseController(CodingMonkeyContext codingMonkeyContext)
        {
            this.CodingMonkeyContext = codingMonkeyContext;
        }

        [HttpGet]
        public JsonResult List()
        {
            var exercises = CodingMonkeyContext.Exercises.Include(e => e.ExerciseExerciseCategories);


            List<ExerciseViewModel> vm = new List<ExerciseViewModel>();
            
            foreach (var exercise in exercises)
            {
                var exerciseTemplate =
                    CodingMonkeyContext.ExerciseTemplates.Include(et => et.Exercise)
                        .SingleOrDefault(et => et.Exercise.ExerciseId == exercise.ExerciseId);

                int exerciseTemplateId = 0;

                if (exerciseTemplate != null)
                {
                    exerciseTemplateId = exerciseTemplate.ExerciseTemplateId;
                }

                vm.Add(new ExerciseViewModel()
                        {
                            Id = exercise.ExerciseId,
                            ExerciseTemplateId = exerciseTemplateId,
                            Guidance = exercise.Guidance,
                            Name = exercise.Name,
                            CategoryIds = GetExerciseCategoryIdsForExercise(exercise.ExerciseId, exercise)
                        });
            }

            return Json(vm);
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Details(int id)
        {
            var exercise =
                CodingMonkeyContext.Exercises.Include(e => e.ExerciseExerciseCategories)
                    .SingleOrDefault(e => e.ExerciseId == id);

            if (exercise == null)
            {
                return Json(string.Empty);
            }

            List<int> exerciseCategoryIds = GetExerciseCategoryIdsForExercise(id, exercise);

            var vm = new ExerciseViewModel()
                             {
                                 Id = exercise.ExerciseId,
                                 Name = exercise.Name,
                                 Guidance = exercise.Guidance,
                                 CategoryIds = exerciseCategoryIds
                             };

            return Json(vm);
        }

        [HttpPost]
        [Authorize]
        public JsonResult Create([FromBody] ExerciseViewModel vm)
        {
            var exceptionResult = new Dictionary<string, dynamic>();

            if (vm == null)
            {
                return Json(string.Empty);
            }

            Exercise exerciseToCreate = new Exercise()
                                            {
                                                Name = vm.Name,
                                                Guidance = vm.Guidance,
                                                ExerciseExerciseCategories =
                                                    new List<ExerciseExerciseCategory>()
                                            };

            try
            {
                CodingMonkeyContext.Exercises.Add(exerciseToCreate);

                if (ModelState.IsValid)
                {
                    CodingMonkeyContext.SaveChanges();
                }

                AddCategoryIds(exerciseToCreate, vm.CategoryIds);

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

            vm.Id = exerciseToCreate.ExerciseId;

            return Json(vm);
        }

        [HttpPost]
        [Route("{id}")]
        [Authorize]
        public JsonResult Update(int id, [FromBody] ExerciseViewModel vm)
        {
            if (vm == null)
            {
                return Json(string.Empty);
            }

            var exceptionResult = new Dictionary<string, dynamic>();
            var exerciseToUpdate =
                CodingMonkeyContext.Exercises.Include(e => e.ExerciseExerciseCategories)
                    .SingleOrDefault(e => e.ExerciseId == id);

            if (exerciseToUpdate == null)
            {
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "record not found";

                return Json(exceptionResult);
            }

            try
            {
                exerciseToUpdate.Name = vm.Name;
                exerciseToUpdate.Guidance = vm.Guidance;

                if (ModelState.IsValid)
                {
                    CodingMonkeyContext.SaveChanges();
                }

                // Update categories
                //Remove categories not in new list
                foreach (var row in exerciseToUpdate.ExerciseExerciseCategories.ToList())
                {
                    if (!vm.CategoryIds.Contains(row.ExerciseCategoryId))
                    {
                        exerciseToUpdate.ExerciseExerciseCategories.Remove(row);
                    }
                }

                //Add categories not currently in db
                foreach (var categoryId in vm.CategoryIds)
                {
                    if (!exerciseToUpdate.ExerciseExerciseCategories.Any(ec => ec.ExerciseCategoryId == categoryId))
                    {
                        exerciseToUpdate.ExerciseExerciseCategories.Add(
                            new ExerciseExerciseCategory()
                                {
                                    ExerciseId = exerciseToUpdate.ExerciseId,
                                    ExerciseCategoryId = categoryId,
                                    ExerciseCategory =
                                        CodingMonkeyContext.ExerciseCategories.SingleOrDefault(
                                            ec => ec.ExerciseCategoryId == categoryId),
                                    Exercise = exerciseToUpdate
                                });
                    }
                }

                CodingMonkeyContext.SaveChanges();
            }
            catch (Exception)
            {
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "exception thrown";

                return Json(exceptionResult);
            }

            vm.CategoryIds =
                exerciseToUpdate.ExerciseExerciseCategories.Where(x => x.ExerciseId == exerciseToUpdate.ExerciseId)
                    .Select(x => x.ExerciseCategoryId)
                    .ToList();

            vm.Id = exerciseToUpdate.ExerciseId;
            vm.Guidance = exerciseToUpdate.Guidance;
            vm.Name = exerciseToUpdate.Name;

            return Json(vm);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            var result = new Dictionary<string, dynamic>();
            var exerciseToDelete =
                CodingMonkeyContext.Exercises.Include(e => e.ExerciseExerciseCategories)
                    .SingleOrDefault(e => e.ExerciseId == id);

            if (exerciseToDelete == null)
            {
                result["deleted"] = false;
                result["reason"] = "record not found";
            }
            else
            {
                try
                {
                    CodingMonkeyContext.Exercises.Remove(exerciseToDelete);
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

        private void AddCategoryIds(Exercise exercise, List<int> categoryIds)
        {
            foreach (int categoryId in categoryIds)
            {
                exercise.ExerciseExerciseCategories.Add(
                    new ExerciseExerciseCategory()
                        {
                            ExerciseId = exercise.ExerciseId,
                            ExerciseCategoryId = categoryId,
                            ExerciseCategory =
                                CodingMonkeyContext.ExerciseCategories.SingleOrDefault(
                                    ec => ec.ExerciseCategoryId == categoryId),
                            Exercise = exercise
                        });
            }
        }

        private List<int> GetExerciseCategoryIdsForExercise(int exerciseId, Exercise exercise)
        {
            return
                exercise.ExerciseExerciseCategories.Where(ec => ec.ExerciseId == exerciseId)
                    .Select(ecid => ecid.ExerciseCategoryId)
                    .ToList();
        }
    }
}
