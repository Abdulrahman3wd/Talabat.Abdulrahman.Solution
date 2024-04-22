using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet("{id}")] // Get : api/basket?id=10
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }
        [HttpPost] // Post /api/basket 
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
           var  createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(basket);
           if (createdOrUpdatedBasket is null ) return BadRequest(new ApiResponse(400));
           return Ok(createdOrUpdatedBasket);
        }
        [HttpDelete] // delete : /api/basket
        public async Task DeleteBasket (string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }


    }
}
