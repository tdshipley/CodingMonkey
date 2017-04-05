namespace CodingMonkey.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CodingMonkey.Models;
    using CodingMonkey.ViewModels;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        public UserManager<ApplicationUser> _userManager { get; set; }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }

        [HttpGet]
        public async Task<JsonResult> CurrentUser()
        {
            var response = new ApplicationUserViewModel();

            try
            {
                var user = await this.GetCurrentUserAsync();

                if (user != null)
                {
                    response.GetUserSucceeded = true;
                    response.Username = user.UserName;
                    response.Roles = user.Roles.ToList();
                }
                else
                {
                    response.GetUserSucceeded = false;
                }
            }
            catch (Exception)
            {
                response.GetUserSucceeded = false;
            }

            return Json(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> ChangePassword([FromBody] ChangePasswordViewModel vm)
        {
            if (vm.NewPassword != vm.NewPasswordConfirmation)
            {
                vm.PasswordChangeSuccessful = false;
                vm.ChangeFailureReason = ChangePasswordViewModel.FailureReason.ConfirmationMismatch;
                return Json(vm);
            }

            try
            {
                var user = await this.GetCurrentUserAsync();

                bool currentPasswordCorrect = await _userManager.CheckPasswordAsync(user, vm.CurrentPassword);

                if (!currentPasswordCorrect)
                {
                    vm.ChangeFailureReason = ChangePasswordViewModel.FailureReason.ValidationError;
                    vm.ChangePasswordErrors = new List<IdentityError>()
                                                  {
                                                      new IdentityError()
                                                          {
                                                              Code = "PasswordMismatch",
                                                              Description = "Current password is incorrect."
                                                          }
                                                  };

                    return Json(vm);
                }

                if (user != null)
                {
                    var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, resetPasswordToken, vm.NewPassword);

                    if (result.Succeeded)
                    {
                        vm.PasswordChangeSuccessful = true;
                        vm.ChangeFailureReason = ChangePasswordViewModel.FailureReason.None;
                        return Json(vm);
                    }
                    else
                    {
                        vm.PasswordChangeSuccessful = false;
                        vm.ChangePasswordErrors = result.Errors;
                        vm.ChangeFailureReason = ChangePasswordViewModel.FailureReason.ValidationError;
                        return Json(vm);
                    }
                }
                else
                {
                    vm.PasswordChangeSuccessful = false;
                    vm.ChangeFailureReason = ChangePasswordViewModel.FailureReason.UserNotFound;
                    return Json(vm);
                }
            }
            catch (Exception)
            {
                vm.PasswordChangeSuccessful = false;
                vm.ChangeFailureReason = ChangePasswordViewModel.FailureReason.Unknown;
                return Json(vm);
            }
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
