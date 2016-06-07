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
                CodingMonkeyContext.Exercises.Add(exerciseToCreate);

                if (ModelState.IsValid) CodingMonkeyContext.SaveChanges();

                exerciseToCreate.RelateExerciseCategoriesToExerciseInMemory(vm.CategoryIds);

                if (ModelState.IsValid) CodingMonkeyContext.SaveChanges();
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

            var existingExercise = CodingMonkeyContext.Exercises
                                                      .Include(e => e.ExerciseExerciseCategories)
                                                      .SingleOrDefault(e => e.ExerciseId == id);

            var updatedExercise = Mapper.Map<Exercise>(vm);

            if (existingExercise == null) return DataActionFailedMessage(DataAction.Updated, DataActionFailReason.RecordNotFound);
            if (updatedExercise == null) return DataActionFailedMessage(DataAction.Updated);

            try
            {

                CodingMonkeyContext.ExerciseExerciseCategories.RemoveRange(existingExercise.ExerciseExerciseCategories);

                if (ModelState.IsValid) CodingMonkeyContext.SaveChanges();

                existingExercise.Name = updatedExercise.Name;
                existingExercise.Guidance = updatedExercise.Guidance;
                existingExercise.ExerciseExerciseCategories = updatedExercise.ExerciseExerciseCategories;

                if (ModelState.IsValid) CodingMonkeyContext.SaveChanges();
            }
            catch (Exception ex)
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
    }
}
