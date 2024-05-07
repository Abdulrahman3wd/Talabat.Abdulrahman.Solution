using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;
using TalabatAPIs.DTOs;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{
	[Authorize]
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
		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		[HttpPost] // POST /api/order
		public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
		{
			var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

			var order = await _orderServices.CreateOrderAsync(orderDto.BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, address);
			if (order is null) return BadRequest(new ApiResponse(400));
			return Ok(_mapper.Map<Order,OrderToReturnDto>(order));
		}
		
		[HttpGet] ///api/orders

		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUserAsync(string email)
		{
			var orders = await _orderServices.GetOrdersForUserAsync(email);
			return Ok(_mapper.Map<IReadOnlyList<Order>,IReadOnlyList< OrderToReturnDto>>(orders));
		}
		
		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("{id}")] // GET // /api/order/1?email=abdulrahman@gmail.com
		public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id , string email)
		{
			var order = await _orderServices.GetOrderByIdForUserAsync(id, email);
			if (order is null) return NotFound(new ApiResponse(404));
		    return	Ok(_mapper.Map<OrderToReturnDto>(order));
		}
		
		[HttpGet("deliverymethods")] // get api/deliverymethods
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
		{
			var deliveryMethos = await _orderServices.GetDeliveryMethodAsync();
			return Ok(deliveryMethos);
		}

	}
}
