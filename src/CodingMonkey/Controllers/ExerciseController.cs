namespace CodingMonkey.Controllers
{
    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;
    using Microsoft.AspNet.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Route("api/[controller]/[action]")]
    public class ExerciseController : ApiController
    {
        [FromServices]
        public CodingMonkeyContext CodingMonkeyContext { get; set; }
        
        [HttpGet]
        public JsonResult List()
        {
            var exercises = CodingMonkeyContext.Exercises;
            
            return Json(exercises);
        }
        
        [HttpGet]
        [Route("{id}")]
        public JsonResult Details(int id)
        {
            var exercise = CodingMonkeyContext.Exercises.SingleOrDefault(e => e.ExerciseId == id);
            
            if (exercise == null)
            {
                return Json(string.Empty);
            }
            
            var result = new ExerciseViewModel()
            {
                Id = exercise.ExerciseId,
                Name = exercise.Name,
                Guidance = exercise.Guidance
            };
            
            return Json(result);
        }
        
        [HttpPost]
        public JsonResult Create([FromBody] ExerciseViewModel model)
        {
            Exercise exercise = new Exercise()
            {
                Name = model.Name,
                Guidance = model.Guidance
            };

            try
            {
                if (ModelState.IsValid)
                {
                    CodingMonkeyContext.Exercises.Add(exercise);
                    CodingMonkeyContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(exercise);
        }
        
        [HttpPost]
        [Route("{id}")]
        public JsonResult Update(int id, [FromBody] ExerciseViewModel model)
        {
            var exceptionResult = new Dictionary<string, dynamic>();
            var exercise = CodingMonkeyContext.Exercises.SingleOrDefault(e => e.ExerciseId == id);
            
            if (exercise == null)
            {
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "record not found";
                
                return Json(exceptionResult);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    exercise.Name = model.Name;
                    exercise.Guidance = model.Guidance;
                    CodingMonkeyContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "exception thrown";
                
                return Json(exceptionResult);
            }

            return Json(exercise);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            var result = new Dictionary<string, dynamic>();
            var exercise = CodingMonkeyContext.Exercises.SingleOrDefault(e => e.ExerciseId == id);
            
            if (exercise == null)
            {
                result["deleted"] = false;
                result["reason"] = "record not found";
            }
            else
            {
                try
                {
                    CodingMonkeyContext.Exercises.Remove(exercise);
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
    }
}
