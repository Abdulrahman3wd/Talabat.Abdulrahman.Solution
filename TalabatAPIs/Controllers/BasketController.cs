using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using TalabatAPIs.DTOs;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{

    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet("{id}")] // Get : api/basket?id=10
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }
        [HttpPost] // Post /api/basket 
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDto , CustomerBasket>(basket);
           var  createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);
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
