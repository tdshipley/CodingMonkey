namespace CodingMonkey.ViewModels
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;

    public class ApplicationUserViewModel
    {
        public bool GetUserSucceeded { get; set; }
        public string Username { get; set; }
        public List<IdentityUserRole<string>> Roles { get; set; }
    }
}
