using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{
	public class PaymentController : BaseApiController
	{
		private readonly IPaymentServices _paymentServices;
		private readonly ILogger<PaymentController> _logger;
		private string whSecret = "whsec_687ac471306966205bfca077ef150bbffa8c8382793b9f73950018ead7db5a9f";

		public PaymentController(IPaymentServices paymentServices , ILogger<PaymentController> logger)
		{
			_paymentServices = paymentServices;
			_logger = logger;
		}

		[ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status400BadRequest)]


		[HttpGet("{basketId}")] // GET : /api/payment/{basketId}

		public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await _paymentServices.CreateOrUpdatePaymentIntent(basketId);
			if (basket is null)
				return BadRequest(new ApiResponse(400, "An Error witj your basket"));
			return Ok(basket);

		}
		[HttpPost("webhook")]
		public async Task<IActionResult> WebHook()
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

			var stripeEvent = EventUtility.ConstructEvent(json,
				Request.Headers["Stripe-Signature"], whSecret);
			var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
			Order? order;
			// Handle the event
			switch (stripeEvent.Type)
			{
				case Events.PaymentIntentSucceeded:
				order =	await _paymentServices.UpdateOrderStatus(paymentIntent.Id, true);
				_logger.LogInformation("Order Is Succeded {0}", order?.PaymentIntentId);
					_logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);
					break;
				case Events.PaymentIntentPaymentFailed:
				order =	await _paymentServices.UpdateOrderStatus(paymentIntent.Id, false);
				_logger.LogInformation("Order Is Faild {0}", order?.PaymentIntentId);
				_logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);

					break;
			}



			return Ok();
		}

	}


}
