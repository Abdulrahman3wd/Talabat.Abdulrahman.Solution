using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services.Contract
{
	public interface IPaymentServices
	{
		Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId);
		Task<Order?> UpdateOrderStatus(string paymentIntetntId , bool isPaid);


	}
}
