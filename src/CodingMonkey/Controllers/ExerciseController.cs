using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingMonkey.Controllers
{
    using CodingMonkey.ViewModels;

    using CodingMonkey.Controllers;
    using Microsoft.AspNet.Mvc;

    public class ExerciseController : ApiController
    {
        [HttpPost]
        public JsonResult Create(ExerciseViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
