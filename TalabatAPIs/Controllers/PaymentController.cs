using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{

	public class PaymentController : BaseApiController
	{
		private readonly IPaymentServices _paymentServices;

		public PaymentController(IPaymentServices paymentServices)
        {
			_paymentServices = paymentServices;
		}

		[ProducesResponseType(typeof(CustomerBasket),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status400BadRequest)]


		[HttpGet("{basketId}")] // GET : /api/payment/{basketId}

		public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await _paymentServices.CreateOrUpdatePaymentIntent(basketId);
			if (basket is null)
				return BadRequest(new ApiResponse(400, "An Error witj your basket"));
			return Ok(basket);

		}
    }
}
