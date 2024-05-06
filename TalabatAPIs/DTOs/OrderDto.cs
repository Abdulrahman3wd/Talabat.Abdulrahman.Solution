﻿using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Order_Aggregate;

namespace TalabatAPIs.DTOs
{
	public class OrderDto
	{
	[Required]
	 public	string BuyerEmail { get; set; }
	[Required]
	public string BasketId { get; set; }
	[Required]
	public int DeliveryMethodId { get; set; }
	public AddressDto ShippingAddress { get; set; }
	}
}
