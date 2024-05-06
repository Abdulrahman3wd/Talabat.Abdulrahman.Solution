using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;

namespace Talabat.Application.OrderServices
{
	public class OrderServices : IOrderServices
	{
		private readonly IBasketRepository _basketRepo;
		private readonly IUnitOfWork _unitOfWork;
		///private readonly IGenericRepository<Product>_productRepo;
		///private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
		///private readonly IGenericRepository<Order> _orderRepo;

		public OrderServices(
			IBasketRepository basketRepo,
			IUnitOfWork unitOfWork
			///IGenericRepository<Product> productRepo,
			///IGenericRepository<DeliveryMethod> deliveryMethodRepo,
			///IGenericRepository<Order> orderRepo)
			)
        {
			_basketRepo = basketRepo;
			_unitOfWork = unitOfWork;

		}
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
		{
			// 1.Get Basket From Baskets Repo
			var basket = await _basketRepo.GetBasketAsync(basketId);


			// 2. Get Selected Items at Basket From Products Repo
			var orderItems = new List<OrderItem>();
			if (basket?.Items?.Count > 0)
			{
                foreach (var item in basket.Items)
                {
					var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
					var productitemOdrerd= new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
					var orderItem = new OrderItem(productitemOdrerd, product.Price, item.Quantity);
					orderItems.Add(orderItem);
                    
                }
            }

			// 3. Calculate SubTotal
			var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

			// 4. Get Delivery Method From DeliveryMethods Repo

			var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

			// 5. Create Order
			var order = new Order(buyerEmail,shippingAddress, deliveryMethodId, orderItems,subtotal);

			_unitOfWork.Repository<Order>().AddAsync(order);

			// 6. Save To Database [TODO]
			var result =await _unitOfWork.CompleteAsync();
			if (result <= 0) return null;
			return order;
		}

		public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Order> GetOrderByIdForUserAsync(string BuyerEmail, int orederId)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
		{
			throw new NotImplementedException();
		}
	}
}
