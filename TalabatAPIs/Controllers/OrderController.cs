using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;
using TalabatAPIs.DTOs;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{

	public class OrderController : BaseApiController
	{
		private readonly IOrderServices _orderServices;
		private readonly IMapper _mapper;

		public OrderController(
			IOrderServices orderServices,
			IMapper mapper)
		{
			_orderServices = orderServices;
			_mapper = mapper;
		}
		[ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
		[HttpPost] // POST /api/order
		public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
		{
			var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

			var order =await _orderServices.CreateOrderAsync(orderDto.BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, address);
			if (order is null) return BadRequest(new ApiResponse(400));
			return Ok(order);
		}

		[HttpGet] ///api/orders

		public async Task<ActionResult<IReadOnlyList<Order >>>GetOrdersForUserAsync(string email)
		{
			var orders = await _orderServices.GetOrdersForUserAsync(email);
			return Ok(orders);
		}
	}
}
