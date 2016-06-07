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
    public class ExerciseTemplateController : BaseController
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
            var exerciseTemplate = CodingMonkeyContext.ExerciseTemplates
                                                      .Include(e => e.Exercise)
                                                      .SingleOrDefault(e => e.Exercise.ExerciseId == exerciseId);

            JsonResult result = exerciseTemplate == null ? Json(string.Empty) : Json(Mapper.Map<ExerciseTemplateViewModel>(exerciseTemplate));

            return result;
        }

        [HttpPost]
        [Authorize]
        public JsonResult Create(int exerciseId, [FromBody] ExerciseTemplateViewModel vm)
        {
            if (vm == null) return Json(string.Empty);
            
            var relatedExercise = CodingMonkeyContext.Exercises.SingleOrDefault(e => e.ExerciseId == exerciseId);

            if (relatedExercise == null) return DataActionFailedMessage(DataAction.Created, DataActionFailReason.RecordNotFound);

            ExerciseTemplate exerciseTemplateToCreate = new ExerciseTemplate()
                                                    {
                                                        InitialCode = vm.InitialCode,
                                                        ClassName = vm.ClassName,
                                                        MainMethodName = vm.MainMethodName,
                                                        MainMethodSignature = vm.MainMethodSignature
                                                    };

            relatedExercise.Template = exerciseTemplateToCreate;

            try
            {
                if (ModelState.IsValid) CodingMonkeyContext.SaveChanges();
            }
            catch (Exception)
            {
                return DataActionFailedMessage(DataAction.Created);
            }

            vm = Mapper.Map<ExerciseTemplateViewModel>(exerciseTemplateToCreate);

            return Json(vm);
        }

        [HttpPost]
        [Authorize]
        public JsonResult Update(int exerciseId, [FromBody] ExerciseTemplateViewModel vm)
        {
            if (vm == null) return Json(string.Empty);
            
            var exerciseTemplateToUpdate = CodingMonkeyContext.ExerciseTemplates
                                                              .Include(et => et.Exercise)
                                                              .SingleOrDefault(e => e.Exercise.ExerciseId == exerciseId);

            if (exerciseTemplateToUpdate == null) return DataActionFailedMessage(DataAction.Updated, DataActionFailReason.RecordNotFound);

            exerciseTemplateToUpdate.ClassName = vm.ClassName;
            exerciseTemplateToUpdate.MainMethodName = vm.MainMethodName;
            exerciseTemplateToUpdate.InitialCode = vm.InitialCode;
            exerciseTemplateToUpdate.MainMethodSignature = vm.MainMethodSignature;

            try
            {
                if (ModelState.IsValid) CodingMonkeyContext.SaveChanges();
            }
            catch (Exception)
            {
                return DataActionFailedMessage(DataAction.Updated);
            }

            vm = Mapper.Map<ExerciseTemplateViewModel>(exerciseTemplateToUpdate);

            return Json(vm);
        }

        [HttpDelete]
        [Authorize]
        public JsonResult Delete(int exerciseId)
        {
            var exerciseTemplate = CodingMonkeyContext.ExerciseTemplates.SingleOrDefault(
                e => e.Exercise.ExerciseId == exerciseId);

            if (exerciseTemplate == null) DataActionFailedMessage(DataAction.Deleted, DataActionFailReason.RecordNotFound);

            try
            {
                CodingMonkeyContext.ExerciseTemplates.Remove(exerciseTemplate);
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