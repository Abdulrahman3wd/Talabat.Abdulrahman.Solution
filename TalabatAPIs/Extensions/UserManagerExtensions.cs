using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace TalabatAPIs.Extensions
{
	public static class UserManagerExtensions
	
	{
		public static async Task<ApplicationUser?> FindUserAddressAysnc(this UserManager<ApplicationUser> userManager , ClaimsPrincipal User )
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var user = await userManager.Users.Include(U=>U.Address).FirstOrDefaultAsync(U=>U.NormalizedEmail==email.ToUpper());
			return user;
			 
		}
	}
}
