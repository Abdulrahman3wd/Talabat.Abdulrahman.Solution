using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Infrastrucure.Identity
{
	public static class ApllicationIdentityDataSeeding
	{
		public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new ApplicationUser()
				{
					DisplayName = "Abdulrahman Awad",
					Email = "abdulrahman@gmail.com",
					UserName = "Abdulrahman3wad",
					PhoneNumber = "01062263100"

				};
				await userManager.CreateAsync(user ,"pass@word1");

			}

		}
	}
}
