namespace CodingMonkey.Controllers
{
    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;
    using Microsoft.AspNet.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Route("api/exercise/{exerciseId}/[controller]/[action]")]
    public class ExerciseTemplateController : ApiController
    {
        [FromServices]
        public CodingMonkeyContext CodingMonkeyContext { get; set; }
        
        [HttpGet]
        [Route("{id}")]
        public JsonResult Details(int exerciseId, int id)
        {
            var exerciseTemplate = CodingMonkeyContext.ExerciseTemplates.SingleOrDefault(e => e.ExerciseTemplateId == id);
            
            if (exerciseTemplate == null)
            {
                return Json(string.Empty);
            }
            
            var relatedExercise = CodingMonkeyContext.Exercises.SingleOrDefault(e => e.ExerciseId == exerciseId);
            
            if(relatedExercise == null)
            {
                return Json(string.Empty);
            }
            
            // TODO: Use navigation properties once implemented in EF7
            // https://github.com/aspnet/EntityFramework/wiki/Roadmap
            var result = new ExerciseTemplateViewModel()
            {
                Id = exerciseTemplate.ExerciseTemplateId,
                ExerciseId = relatedExercise.ExerciseId,
                InitialCode = exerciseTemplate.InitialCode,
                ClassName = exerciseTemplate.ClassName,
                MainMethodName = exerciseTemplate.MainMethodName
            };
            
            return Json(result);
        }
        
        [HttpPost]
        public JsonResult Create(int exerciseId, [FromBody] ExerciseTemplateViewModel model)
        {
            var exceptionResult = new Dictionary<string, dynamic>();
            var exercise = CodingMonkeyContext.Exercises.SingleOrDefault(e => e.ExerciseId == exerciseId);
            
            if (exercise == null)
            {
                exceptionResult["created"] = false;
                exceptionResult["reason"] = "exercise not found";
                
                return Json(exceptionResult);
            }
            
            ExerciseTemplate exerciseTemplate = new ExerciseTemplate()
            {
                InitialCode = model.InitialCode,
                ClassName = model.ClassName,
                MainMethodName = model.MainMethodName
            };
            
            exercise.Template = exerciseTemplate;
            
            try
            {
                if (ModelState.IsValid)
                {
                    CodingMonkeyContext.SaveChanges();
                }
            }
            catch (System.Exception)
            {
                exceptionResult["created"] = false;
                exceptionResult["reason"] = "exception saving to db";
                
                return Json(exceptionResult);
            }
            
            model.Id = exercise.Template.ExerciseTemplateId;
            model.ExerciseId = exercise.ExerciseId;

            return Json(model);
        }
        
        [HttpPost]
        [Route("{id}")]
        public JsonResult Update(int exerciseId, int id, [FromBody] ExerciseTemplateViewModel model)
        {
            var exceptionResult = new Dictionary<string, dynamic>();
            var exerciseTemplate = CodingMonkeyContext.ExerciseTemplates.SingleOrDefault(e => e.ExerciseTemplateId == id);
            
            if (exerciseTemplate == null)
            {
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "record not found";
                
                return Json(exceptionResult);
            }
            
            exerciseTemplate.ClassName = model.ClassName;
            exerciseTemplate.MainMethodName = model.MainMethodName;
            exerciseTemplate.InitialCode = model.InitialCode;

            try
            {
                if (ModelState.IsValid)
                {
                    CodingMonkeyContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "exception thrown";
                
                return Json(exceptionResult);
            }

            model.Id = exerciseTemplate.ExerciseTemplateId;
            
            // TODO: Use navigation properties once implemented in EF7
            // https://github.com/aspnet/EntityFramework/wiki/Roadmap
            var relatedExercise = CodingMonkeyContext.Exercises.SingleOrDefault(e => e.ExerciseId == exerciseId);    
            model.ExerciseId = relatedExercise.ExerciseId;

            return Json(model);
        }
        
        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int exerciseId, int id)
        {
            var result = new Dictionary<string, dynamic>();
            var exerciseTemplate = CodingMonkeyContext.ExerciseTemplates.SingleOrDefault(e => e.ExerciseTemplateId == id);
            
            if (exerciseTemplate == null)
            {
                result["deleted"] = false;
                result["reason"] = "record not found";
            }
            else
            {
                try
                {
                    CodingMonkeyContext.ExerciseTemplates.Remove(exerciseTemplate);
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