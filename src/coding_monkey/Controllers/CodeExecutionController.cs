using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace coding_monkey.Controllers
{
    public class CodeExecutionController : ApiController
    {
        public IActionResult Execute()
        {
            return Json("Hello");
        }
    }
}
