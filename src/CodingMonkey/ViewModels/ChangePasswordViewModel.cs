namespace CodingMonkey.ViewModels
{
    using System.Collections.Generic;

    using Microsoft.AspNet.Identity;

    public class ChangePasswordViewModel
    {
        public enum FailureReason
        {
            ConfirmationMismatch = 100,
            UserNotFound = 200,
            ValidationError = 300,
            Unknown = 900,
            None = 000
        }

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }
        public bool PasswordChangeSuccessful { get; set; }
        public FailureReason ChangeFailureReason { get; set; }
        public IEnumerable<IdentityError> ChangePasswordErrors { get; set; } 
    }
}
