using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace CodingMonkey.Controllers
{
    public class CodeExecutionController : ApiController
    {
        public IActionResult Execute()
        {
            string compilerCode = @"// A Hello World! program in C#.
                                    using System;
                                    namespace HelloWorld
                                        {
                                            class Hello
                                            {
                                                static void Main()
                                                {
                                                    Console.WriteLine(""Hello World!"");
                                            }
                                        }
                                    }";

            return Json("Hello");
        }
    }
}
