﻿namespace Talabat.Core.Entities.Identity
{
	public class UserAddress : BaseEntity
	{
        
		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string Street { get; set; } = null!;
		public string City { get; set; } = null!;
		public string Country { get; set; } = null!;
		public string ApplicationUserId { get; set; } // Foreign Key
		public ApplicationUser User { get; set; } = null!; // Navigetional Property [ONE]
	}
}