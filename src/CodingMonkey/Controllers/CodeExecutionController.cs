namespace CodingMonkey.Controllers
{
    using System.Collections.Generic;
    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;
    using Microsoft.AspNet.Mvc;
    using CodingMonkey.CodeExecutor;
    using System.Linq;

    using Microsoft.Data.Entity;

    [Route("api/[controller]/[action]/{id}")]
    public class CodeExecutionController : Controller
    {
        [FromServices]
        public CodingMonkeyContext CodingMonkeyContext { get; set; }

        [HttpPost]
        public JsonResult Compile(int id, [FromBody] CodeEditorViewModel model)
        {
            var result = RoslynCompiler.Compile(model.Code);

            if (result == null || result.Count == 0)
            {
                model.HasErrors = false;
                model.Errors = null;
            }
            else
            {
                model.HasErrors = true;
                model.Errors = new List<CompilerErrorViewModel>();

                foreach (var resultError in result)
                {
                    model.Errors.Add(new CompilerErrorViewModel()
                    {
                        Id = resultError.Id,
                        Severity = resultError.Severity,
                        Message = resultError.Message,
                        LineNumberStart = resultError.StartLineNumber,
                        LineNumberEnd = resultError.EndLineNumber,
                        ColStart = resultError.ColStart,
                        ColEnd = resultError.ColEnd,
                        ErrorLength = resultError.ErrorLength
                    });
                }
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult Execute(int id, [FromBody] CodeEditorViewModel model)
        {
            var exercise = CodingMonkeyContext.Exercises
                                              .Include(e => e.Template)
                                              .Include(e => e.Tests).ThenInclude(t => t.TestInputs)
                                              .SingleOrDefault(e => e.ExerciseId == id);

            if (exercise == null || exercise.Template == null)
            {
                return Json(string.Empty);
            }

            // Just *testing* (ha) that this works
            var firstTest = exercise.Tests.First();
            
            Dictionary<string, object> inputs = new Dictionary<string, dynamic>(); 
            foreach (var item in firstTest.TestInputs)
            {
                switch (item.ValueType)
                {
                  case "String":
                  {
                      inputs.Add(item.ArgumentName, item.ValueType);
                      break;
                  }
                  case "Integer":
                  {
                      int inputValue;
                      int.TryParse(item.Value, out inputValue);
                      inputs.Add(item.ArgumentName, inputValue);
                      break;
                  }
                  case "Boolean":
                  {
                      bool inputValue;
                      bool.TryParse(item.Value, out inputValue);
                      inputs.Add(item.ArgumentName, inputValue);
                      break;
                  }
                  default:
                  {
                      return Json(string.Empty);
                  }
                }
                inputs.Add(item.ArgumentName, item.Value);
            }

            model.Output = RoslynCompiler.Execute(model.Code,
                                                  exercise.Template.ClassName,
                                                  exercise.Template.MainMethodName,
                                                  inputs);

            return Json(model);
        }
    }
}
