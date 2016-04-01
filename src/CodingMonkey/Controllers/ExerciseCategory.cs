namespace CodingMonkey.Controllers
{
    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;
    using Microsoft.AspNet.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Data.Entity;

    [Route("api/[controller]/[action]")]
    public class ExerciseCategoryController : Controller
    {
        [FromServices]
        public CodingMonkeyContext CodingMonkeyContext { get; set; }
        
        [HttpGet]
        public JsonResult List()
        {
            var exerciseCategories = CodingMonkeyContext.ExerciseCategories;
            
            return Json(exerciseCategories);
        }
        
        [HttpGet]
        [Route("{id}")]
        public JsonResult Details(int id)
        {
            var exerciseCategory = CodingMonkeyContext.ExerciseCategories.SingleOrDefault(e => e.ExerciseCategoryId == id);
            
            if (exerciseCategory == null)
            {
                return Json(string.Empty);
            }
            
            List<int> exercisesInCategory = GetExercisesInCategory(exerciseCategory.ExerciseCategoryId);
            
            var result = new ExerciseCategoryViewModel()
            {
                Id = exerciseCategory.ExerciseCategoryId,
                Name = exerciseCategory.Name,
                Description = exerciseCategory.Description,
                ExerciseIds = exercisesInCategory
            };
            
            return Json(result);
        }
        
        [HttpPost]
        public JsonResult Create([FromBody] ExerciseCategoryViewModel model)
        {
            ExerciseCategory exerciseCategory = new ExerciseCategory()
            {
                Name = model.Name,
                Description = model.Description
            };

            try
            {
                if (ModelState.IsValid)
                {
                    CodingMonkeyContext.ExerciseCategories.Add(exerciseCategory);
                    CodingMonkeyContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            List<int> exercisesInCategory = GetExercisesInCategory(exerciseCategory.ExerciseCategoryId);

            model.Id = exerciseCategory.ExerciseCategoryId;
            model.Name = exerciseCategory.Name;
            model.Description = exerciseCategory.Description;
            model.ExerciseIds = exercisesInCategory;

            return Json(model);
        }
        
        [HttpPost]
        [Route("{id}")]
        public JsonResult Update(int id, [FromBody] ExerciseCategoryViewModel model)
        {
            var exceptionResult = new Dictionary<string, dynamic>();
            var exerciseCategory = CodingMonkeyContext.ExerciseCategories.SingleOrDefault(e => e.ExerciseCategoryId == id);
            
            if (exerciseCategory == null)
            {
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "record not found";
                
                return Json(exceptionResult);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    exerciseCategory.Name = model.Name;
                    exerciseCategory.Description = model.Description;
                    CodingMonkeyContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "exception thrown";
                
                return Json(exceptionResult);
            }
            
            List<int> exercisesInCategory = GetExercisesInCategory(exerciseCategory.ExerciseCategoryId);
            
            model.Id = exerciseCategory.ExerciseCategoryId;
            model.Name = exerciseCategory.Name;
            model.Description = exerciseCategory.Description;
            model.ExerciseIds = exercisesInCategory;

            return Json(model);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            var result = new Dictionary<string, dynamic>();
            var exerciseCategory = CodingMonkeyContext.ExerciseCategories.SingleOrDefault(e => e.ExerciseCategoryId == id);
            
            if (exerciseCategory == null)
            {
                result["deleted"] = false;
                result["reason"] = "record not found";
            }
            else
            {
                try
                {
                    CodingMonkeyContext.ExerciseCategories.Remove(exerciseCategory);
                    CodingMonkeyContext.SaveChanges();
                    result["deleted"] = true;
                }
                catch(Exception ex)
                {
                    result["deleted"] = false;
                    result["reason"] = "exception thrown";
                }
            }
            
            return Json(result);
        }
        
        private List<int> GetExercisesInCategory(int exerciseCategoryId)
        {
            return CodingMonkeyContext.Exercises
                        .Include(e => e.ExerciseExerciseCategories)
                        .Where(e => e.ExerciseExerciseCategories.
                                Any( c => c.ExerciseCategoryId == exerciseCategoryId)
                                )
                        .Select(e => e.ExerciseId)
                        .ToList();
        }
    }
}