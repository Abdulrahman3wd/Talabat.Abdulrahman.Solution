using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
	public class OrderItem : BaseEntity
	{
		private OrderItem()
		{

		}
		public OrderItem(ProductItemOrder product, decimal price, int quantity)
		{
			Product = product;
			Price = price;
			Quantity = quantity;
		}

		public ProductItemOrder Product { get; set; } = null!;
		public decimal Price { get; set; }
		public int Quantity { get; set; }	
	}
}
