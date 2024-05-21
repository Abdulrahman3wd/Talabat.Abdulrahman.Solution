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
using Talabat.Core.Specifications.OrderSpecs;

namespace Talabat.Application.OrderServices
{
	public class OrderServices : IOrderServices
	{
		private readonly IBasketRepository _basketRepo;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPaymentServices _paymentServices;

		///private readonly IGenericRepository<Product>_productRepo;
		///private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
		///private readonly IGenericRepository<Order> _orderRepo;

		public OrderServices(
			IBasketRepository basketRepo,
			IUnitOfWork unitOfWork,
			IPaymentServices paymentServices
			///IGenericRepository<Product> productRepo,
			///IGenericRepository<DeliveryMethod> deliveryMethodRepo,
			///IGenericRepository<Order> orderRepo)
			)
        {
			_basketRepo = basketRepo;
			_unitOfWork = unitOfWork;
			_paymentServices = paymentServices;
		}
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
		{
			// 1.Get Basket From Baskets Repo
			var basket = await _basketRepo.GetBasketAsync(basketId);


			// 2. Get Selected Items at Basket From Products Repo
			var orderItems = new List<OrderItem>();
			if (basket?.Items?.Count > 0)
			{
				var productrepository = _unitOfWork.Repository<Product>();
				foreach (var item in basket.Items)
                {

					var product = await productrepository.GetByIdAsync(item.Id);
					 
					var productitemOdrerd= new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
					var orderItem = new OrderItem(productitemOdrerd, product.Price, item.Quantity);
					orderItems.Add(orderItem);
                    
                }
            }

			// 3. Calculate SubTotal
			var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

			// 4. Get Delivery Method From DeliveryMethods Repo

			var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
			var OrderRepo = _unitOfWork.Repository < Order >();
			var spec = new OrderWithpaymentIntentSpecification(basket?.PaymentIntentId);
			var existingOrder = await OrderRepo.GetByIdWithSpecAsync(spec);
			if(existingOrder is not null)
			{
				OrderRepo.Delete(existingOrder);
				await _paymentServices.CreateOrUpdatePaymentIntent(basketId);
			}

			// 5. Create Order

			var order = new Order(buyerEmail,shippingAddress, deliveryMethodId, orderItems,subtotal,basket?.PaymentIntentId??"");

			OrderRepo.AddAsync(order);

			// 6. Save To Database [TODO]
			var result =await _unitOfWork.CompleteAsync();
			if (result <= 0) return null;
			return order;
		}

		public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
			=> await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();


		public Task<Order?> GetOrderByIdForUserAsync(int orderId ,string BuyerEmail)
		{
			var orderRepo = _unitOfWork.Repository<Order>();

			var orderSpec = new OrderSpecifications(orderId, BuyerEmail);

			var order = orderRepo.GetByIdWithSpecAsync(orderSpec);

			return order;
		}

		public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
		{
			var ordersRepo = _unitOfWork.Repository<Order>();

			var spec = new OrderSpecifications(buyerEmail);

			var orders =await ordersRepo.GetAllWithSpecAsync(spec);
			return orders;
		}
	}
}
