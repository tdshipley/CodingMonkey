using System;
using System.Collections.Generic;
using System.Linq;
//using System.Security.Permissions;
//using System.Security.Policy;
using System.Threading.Tasks;
using CodingMonkey.ViewModels;
using Microsoft.AspNet.Mvc;
using CodingMonkey.CodeExecutor;

namespace CodingMonkey.Controllers
{
    public class CodeExecutionController : ApiController
    {
        [HttpPost]
        public Task<JsonResult> Compile([FromBody] CodeEditorViewModel model)
        {
            var result = RoslynCompiler.Compile(model.Code).Result;

            if (result == null || result.Count == 0)
            {
                model.HasErrors = false;
                model.Errors = null;
            }
            else
            {
                model.HasErrors = true;
                model.Errors = result;
            }

            return Task.FromResult(Json(model));
        }
    }
}
