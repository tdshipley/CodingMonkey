namespace CodingMonkey.ViewModels
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class ApplicationUserViewModel
    {
        public bool GetUserSucceeded { get; set; }
        public string Username { get; set; }
        public List<IdentityUserRole<string>> Roles { get; set; }
    }
}
