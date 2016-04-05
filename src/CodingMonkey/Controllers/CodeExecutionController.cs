namespace CodingMonkey.Controllers
{
    using System.Collections.Generic;
    using CodingMonkey.ViewModels;
    using Microsoft.AspNet.Mvc;
    using CodingMonkey.CodeExecutor;

    [Route("api/[controller]/[action]")]
    public class CodeExecutionController : Controller
    {
        [HttpPost]
        public JsonResult Compile([FromBody] CodeEditorViewModel model)
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
        public JsonResult Execute([FromBody] CodeEditorViewModel model)
        {
            model.Output = RoslynCompiler.Execute(model.Code);

            return Json(model);
        }
    }
}
