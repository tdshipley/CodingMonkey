namespace CodingMonkey.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CodingMonkey.Models;
    using CodingMonkey.ViewModels;

    using Microsoft.AspNet.Authorization;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Mvc;

    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        [FromServices]
        public UserManager<ApplicationUser> _userManager { get; set; }

        [HttpGet]
        [Authorize]
        public async Task<JsonResult> CurrentUser()
        {
            var response = new ApplicationUserViewModel();

            try
            {
                var user = await this.GetCurrentUserAsync();

                response.GetUserSucceeded = true;
                response.Username = user.UserName;
                response.Roles = user.Roles.ToList();
            }
            catch (Exception ex)
            {
                response.GetUserSucceeded = false;
            }

            return Json(response);
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.FindByIdAsync(HttpContext.User.GetUserId());
        }
    }
}
