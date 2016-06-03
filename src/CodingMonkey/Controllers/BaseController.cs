using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CodingMonkey.Controllers
{
    public class BaseController : Controller
    {
        protected JsonResult ResultNotFoundMessage(ControllerDataActions dataAction)
        {
            Dictionary<string, dynamic> result = new Dictionary<string, dynamic>
                                                     {
                                                        { dataAction.ToString().ToLower(), false },
                                                        { "reason", "exception thrown"}
                                                     };

            return Json(result);
        }

        protected enum ControllerDataActions
        {
            Deleted = 100,
            Updated = 200
        }
    }
}