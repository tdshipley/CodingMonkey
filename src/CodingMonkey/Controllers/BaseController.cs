using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CodingMonkey.Controllers
{
    public class BaseController : Controller
    {
        protected JsonResult DataActionFailedMessage(DataAction dataAction,
            DataActionFailReason reason = DataActionFailReason.ExceptionThrown)
        {
            Dictionary<string, dynamic> result = new Dictionary<string, dynamic>
                                                     {
                                                        { dataAction.ToString().ToLower(), false },
                                                        { "reason", reason.ToString() }
                                                     };

            return Json(result);
        }

        protected enum DataAction
        {
            Deleted = 100,
            Updated = 200,
            Created = 300
        }

        protected enum DataActionFailReason
        {
            ExceptionThrown = 100,
            RecordNotFound = 200
        }
    }
}