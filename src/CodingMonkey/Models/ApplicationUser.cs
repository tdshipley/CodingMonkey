namespace CodingMonkey.Models
{
	using Microsoft.AspNetCore.Identity;
	using System.Collections.Generic;

	public class ApplicationUser : IdentityUser
    {
		/// <summary>
		/// Navigation property for the roles this user belongs to.
		/// Removed in V1 Identity - See https://docs.microsoft.com/en-us/aspnet/core/migration/1x-to-2x/identity-2x
		/// </summary>
		public virtual ICollection<IdentityUserRole<string>> Roles { get; } = new List<IdentityUserRole<string>>();
	}
}
