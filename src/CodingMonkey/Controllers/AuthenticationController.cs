namespace CodingMonkey.Controllers
{
    using System;

    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Mvc;

    [Route("api/[controller]/[action]")]
    public class AuthenticationController
    {
        [FromServices]
        private SignInManager<ApplicationUser> _signInManager { get; set; }

        [HttpPost]
        public JsonResult Login()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public JsonResult Logout()
        {
            throw new NotImplementedException();
        }

    }
}
