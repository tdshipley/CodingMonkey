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

    [Route("api/[controller]/[action]")]
    public class ExerciseController : BaseController
    {
        public CodingMonkeyContext CodingMonkeyContext { get; set; }

        public IMapper Mapper { get; set; }

        public ExerciseController(CodingMonkeyContext codingMonkeyContext, IMapper mapper)
        {
            this.CodingMonkeyContext = codingMonkeyContext;
            this.Mapper = mapper;
        }

        [HttpGet]
        public JsonResult List()
        {
            var exercises = CodingMonkeyContext.Exercises
                                               .Include(e => e.ExerciseExerciseCategories)
                                               .Include(e => e.Template)
                                               .ToList();


            var vm = Mapper.Map<List<ExerciseViewModel>>(exercises);

            return Json(vm);
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Details(int id)
        {
            var exercise = CodingMonkeyContext.Exercises
                                              .Include(e => e.ExerciseExerciseCategories)
                                              .Include(e => e.Template)
                                              .SingleOrDefault(e => e.ExerciseId == id);

            if (exercise == null) return Json(string.Empty);

            var vm = Mapper.Map<ExerciseViewModel>(exercise);

            return Json(vm);
        }

        [HttpPost]
        [Authorize]
        public JsonResult Create([FromBody] ExerciseViewModel vm)
        {
            if (vm == null) return Json(string.Empty);

            Exercise exerciseToCreate = new Exercise()
                                            {
                                                Name = vm.Name,
                                                Guidance = vm.Guidance,
                                                ExerciseExerciseCategories = new List<ExerciseExerciseCategory>()
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

            var exerciseToUpdate =
                CodingMonkeyContext.Exercises.Include(e => e.ExerciseExerciseCategories)
                    .SingleOrDefault(e => e.ExerciseId == id);

            if (exerciseToUpdate == null) return DataActionFailedMessage(DataAction.Updated, DataActionFailReason.RecordNotFound);

            try
            {
                exerciseToUpdate.Name = vm.Name;
                exerciseToUpdate.Guidance = vm.Guidance;

                if (ModelState.IsValid) CodingMonkeyContext.SaveChanges();

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
                return DataActionFailedMessage(DataAction.Updated);
            }

            vm.CategoryIds =
                exerciseToUpdate.ExerciseExerciseCategories.Where(x => x.ExerciseId == exerciseToUpdate.ExerciseId)
                    .Select(x => x.ExerciseCategoryId)
                    .ToList();

            vm = Mapper.Map<ExerciseViewModel>(exerciseToUpdate);

            return Json(vm);
        }

        [HttpDelete]
        [Authorize]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            var exerciseToDelete = CodingMonkeyContext.Exercises
                                                      .Include(e => e.ExerciseExerciseCategories)
                                                      .SingleOrDefault(e => e.ExerciseId == id);

            if (exerciseToDelete == null) return DataActionFailedMessage(DataAction.Deleted, DataActionFailReason.RecordNotFound);

            try
            {
                CodingMonkeyContext.Exercises.Remove(exerciseToDelete);
                CodingMonkeyContext.SaveChanges();
            }
            catch (Exception)
            {
                return DataActionFailedMessage(DataAction.Deleted);
            }

            return Json(new { deleted = true });
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
    }
}
