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

    [Route("api/exercise/{exerciseId}/[controller]/[action]")]
    public class ExerciseTemplateController : Controller
    {
        public CodingMonkeyContext CodingMonkeyContext { get; set; }

        public IMapper Mapper { get; set; }

        public ExerciseTemplateController(CodingMonkeyContext codingMonkeyContext, IMapper mapper)
        {
            this.CodingMonkeyContext = codingMonkeyContext;
            this.Mapper = mapper;
        }

        [HttpGet]
        public JsonResult Details(int exerciseId)
        {
            var exerciseTemplate =
                CodingMonkeyContext.ExerciseTemplates.Include(e => e.Exercise)
                    .SingleOrDefault(e => e.Exercise.ExerciseId == exerciseId);

            if (exerciseTemplate == null)
            {
                return Json(string.Empty);
            }

            var vm = this.Mapper.Map<ExerciseTemplateViewModel>(exerciseTemplate);

            return Json(vm);
        }

        [HttpPost]
        [Authorize]
        public JsonResult Create(int exerciseId, [FromBody] ExerciseTemplateViewModel vm)
        {
            if (vm == null)
            {
                return Json(string.Empty);
            }

            var exceptionResult = new Dictionary<string, dynamic>();
            var relatedExercise = CodingMonkeyContext.Exercises.SingleOrDefault(e => e.ExerciseId == exerciseId);

            if (relatedExercise == null)
            {
                exceptionResult["created"] = false;
                exceptionResult["reason"] = "exercise not found";

                return Json(exceptionResult);
            }

            ExerciseTemplate exerciseTemplate = new ExerciseTemplate()
                                                    {
                                                        InitialCode = vm.InitialCode,
                                                        ClassName = vm.ClassName,
                                                        MainMethodName = vm.MainMethodName,
                                                        MainMethodSignature = vm.MainMethodSignature
                                                    };

            relatedExercise.Template = exerciseTemplate;

            try
            {
                if (ModelState.IsValid)
                {
                    CodingMonkeyContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                exceptionResult["created"] = false;
                exceptionResult["reason"] = "exception saving to db";

                return Json(exceptionResult);
            }

            vm.Id = relatedExercise.Template.ExerciseTemplateId;
            vm.ExerciseId = relatedExercise.ExerciseId;

            return Json(vm);
        }

        [HttpPost]
        [Authorize]
        public JsonResult Update(int exerciseId, [FromBody] ExerciseTemplateViewModel vm)
        {
            if (vm == null)
            {
                return Json(string.Empty);
            }

            var exceptionResult = new Dictionary<string, dynamic>();
            var exerciseTemplate =
                CodingMonkeyContext.ExerciseTemplates.Include(et => et.Exercise)
                    .SingleOrDefault(e => e.Exercise.ExerciseId == exerciseId);

            if (exerciseTemplate == null)
            {
                exceptionResult["updated"] = false;
                exceptionResult["reason"] = "record not found";

                return Json(exceptionResult);
            }

            exerciseTemplate.ClassName = vm.ClassName;
            exerciseTemplate.MainMethodName = vm.MainMethodName;
            exerciseTemplate.InitialCode = vm.InitialCode;
            exerciseTemplate.MainMethodSignature = vm.MainMethodSignature;

            try
            {
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

            vm.Id = exerciseTemplate.ExerciseTemplateId;
            vm.ExerciseId = exerciseTemplate.Exercise.ExerciseId;

            return Json(vm);
        }

        [HttpDelete]
        [Authorize]
        public JsonResult Delete(int exerciseId)
        {
            var result = new Dictionary<string, dynamic>();
            var exerciseTemplate = CodingMonkeyContext.ExerciseTemplates.SingleOrDefault(
                e => e.Exercise.ExerciseId == exerciseId);

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
                catch (Exception)
                {
                    result["deleted"] = false;
                    result["reason"] = "exception thrown";
                }
            }

            return Json(result);
        }
    }
}