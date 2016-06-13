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

    using CodingMonkey.Models.Repositories;

    [Route("api/exercise/{exerciseId}/[controller]/[action]")]
    public class ExerciseTemplateController : BaseController
    {
        public CodingMonkeyRepositoryContext CodingMonkeyRepositoryContext { get; set; }

        public IMapper Mapper { get; set; }

        public ExerciseTemplateController(CodingMonkeyRepositoryContext codingMonkeyRepositoryContext, IMapper mapper)
        {
            this.CodingMonkeyRepositoryContext = codingMonkeyRepositoryContext;
            this.Mapper = mapper;
        }

        [HttpGet]
        public JsonResult Details(int exerciseId)
        {
            var exerciseTemplate = this.CodingMonkeyRepositoryContext.ExerciseTemplateRepository.GetById(exerciseId);

            JsonResult result = exerciseTemplate == null ? Json(string.Empty) : Json(Mapper.Map<ExerciseTemplateViewModel>(exerciseTemplate));

            return result;
        }

        [HttpPost]
        [Authorize]
        public JsonResult Create(int exerciseId, [FromBody] ExerciseTemplateViewModel vm)
        {
            if (vm == null) return Json(string.Empty);

            var exerciseTemplateToCreate = Mapper.Map<ExerciseTemplate>(vm);

            try
            {
                if (ModelState.IsValid)
                {
                    exerciseTemplateToCreate = this.CodingMonkeyRepositoryContext.ExerciseTemplateRepository
                                                                                 .Create(exerciseId, exerciseTemplateToCreate);
                }
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

            ExerciseTemplate exerciseTemplateToUpdate = Mapper.Map<ExerciseTemplate>(vm);

            try
            {
                if (ModelState.IsValid)
                {
                    exerciseTemplateToUpdate = CodingMonkeyRepositoryContext.ExerciseTemplateRepository
                                                                            .Update(exerciseId, exerciseTemplateToUpdate.ExerciseTemplateId, exerciseTemplateToUpdate);
                }
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
            try
            {
                CodingMonkeyRepositoryContext.ExerciseRepository.Delete(exerciseId);
            }
            catch (Exception)
            {
                return DataActionFailedMessage(DataAction.Deleted);
            }

            return Json(new { deleted = true });
        }
    }
}