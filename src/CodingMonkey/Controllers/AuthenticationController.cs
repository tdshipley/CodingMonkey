namespace CodingMonkey.Controllers
{
    using System;
    using System.Threading.Tasks;

    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]/[action]")]
    public class AuthenticationController : Controller
    {
        public SignInManager<ApplicationUser> _signInManager { get; set; }

        public AuthenticationController(SignInManager<ApplicationUser> signInManager)
        {
            this._signInManager = signInManager;
        }

        [HttpPost]
        public async Task<JsonResult> Login([FromBody]LoginViewModel vm)
        {
            var response = new LoginResultViewModel();

            try
            {
                var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, lockoutOnFailure: false);
                response.LoginSucceeded = result.Succeeded;
            }
            catch (Exception)
            {
                response.LoginSucceeded = false;
            }

            return Json(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> Logout()
        {
            var response = new LoginResultViewModel();
            try
            {
                await _signInManager.SignOutAsync();
                response.LogoutSucceeded = true;
            }
            catch (Exception)
            {
                response.LogoutSucceeded = true;
            }

            return Json(response);
        }

    }
}
