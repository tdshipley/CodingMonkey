namespace CodingMonkey.Controllers
{
    using System;
    using System.Threading.Tasks;

    using CodingMonkey.ViewModels;
    using CodingMonkey.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Mvc;

    [Route("api/[controller]/[action]")]
    public class AuthenticationController : Controller
    {
        [FromServices]
        public SignInManager<ApplicationUser> _signInManager { get; set; }

        [HttpPost]
        public async Task<JsonResult> Login([FromBody]LoginViewModel vm)
        {
            var response = new LoginResultViewModel();

            try
            {
                var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, lockoutOnFailure: false);
                response.LoginSucceeded = result.Succeeded;
            }
            catch (Exception ex)
            {
                response.LoginSucceeded = false;
            }

            return Json(response);
        }

        [HttpPost]
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
