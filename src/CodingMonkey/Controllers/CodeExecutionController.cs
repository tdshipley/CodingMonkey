using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading.Tasks;
using coding_monkey.ViewModels;
using CodingMonkey.CodeExecutor;
using Microsoft.AspNet.Mvc;

namespace CodingMonkey.Controllers
{
    public class CodeExecutionController : ApiController
    {
        [HttpPost]
        public IActionResult Compile([FromBody] CodeEditorViewModel model)
        {
            List<string> assemblies = new List<string>()
            {
                "System.dll"
            };

            Compiler codeCompiler = new Compiler(model.Code, assemblies, false);
            var compiledCode = codeCompiler.CompileFromSource();

            if (compiledCode.Errors.HasErrors)
            {
                model.HasErrors = true;
                model.Errors = compiledCode.Errors;
                return Json(model);
            }

            model.HasErrors = false;
            return Json(model);
        }
    }
}
