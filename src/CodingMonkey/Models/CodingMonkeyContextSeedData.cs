namespace CodingMonkey.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;

    public class CodingMonkeyContextSeedData
    {
        private CodingMonkeyContext _context;

        private UserManager<ApplicationUser> _userManager;

        public CodingMonkeyContextSeedData(CodingMonkeyContext context, UserManager<ApplicationUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public async Task EnsureSeedDataAsync()
        {
            if (await this._userManager.FindByEmailAsync("thomas.shipley@googlemail.com") == null)
            {
                // Add user
                var user = new ApplicationUser() { UserName = "tdshipley", Email = "thomas.shipley@googlemail.com" };

                await this._userManager.CreateAsync(user, "password!");
            }
        }
    }
}
