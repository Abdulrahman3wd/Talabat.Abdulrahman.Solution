using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Infrastrucure.Data.Config.OrderConfig
{
	internal class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(order => order.ShippingAddress, shippingAddress => shippingAddress.WithOwner());
			builder.Property(order => order.Status)
				.HasConversion(
				(OStatus) => OStatus.ToString(),
				(OStatus) => (OrderStatus) Enum.Parse(typeof(OrderStatus),OStatus)
				);
			builder.Property(order => order.Subtotal)
				.HasColumnType("decimal(12,2)"); 

				

		}
	}
}
