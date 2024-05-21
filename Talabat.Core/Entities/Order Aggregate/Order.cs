using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
	public class Order : BaseEntity
	{
        // there is must be parameterless Constructor For EFCore
        private Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, int? deliveryMethodId, ICollection<OrderItem> items, decimal subtotal , string paymentIntentId)
		{
			BuyerEmail = buyerEmail;
			ShippingAddress = shippingAddress;
			DeliveryMethodId = deliveryMethodId;
			Items = items;
			Subtotal = subtotal;
			PaymentIntentId = paymentIntentId;
		}

		public string BuyerEmail { get; set; } = null!;
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
		public OrderStatus Status { get; set; } = OrderStatus.Pending;
		public Address ShippingAddress { get; set; } = null!;

		public int? DeliveryMethodId { get; set; } //Forign Key
		public DeliveryMethod? DeliveryMethod { get; set; } = null!; //Navigational property[One]
		public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();  //Navigational property[Many]
		public decimal Subtotal { get; set; }

		//[NotMapped]
		//public decimal Total => Subtotal + DeliveryMethod.Cost; 
		public decimal GetTotal()=>Subtotal + DeliveryMethod.Cost;
		public string PaymentIntentId { get; set; } 


    }
}
