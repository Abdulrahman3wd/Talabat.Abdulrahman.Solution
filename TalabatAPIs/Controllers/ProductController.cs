using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpecs;
using TalabatAPIs.DTOs;

namespace TalabatAPIs.Controllers
{

    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IGenericRepository<Product> productRepository , IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        // /api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetProducts()
        {
            var sepc = new ProductWithBrandAndCategorySpecifications();
            var products = await _productRepository.GetAllWithSpecAsync(sepc);
            return Ok(_mapper.Map<IEnumerable<Product> ,IEnumerable< ProductToReturnDto>>(products));
        }

        //api/products/10
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var sepc = new ProductWithBrandAndCategorySpecifications(id);
            var product = await _productRepository.GetWithSpecAsync(sepc);
            if (product is null)
                return NotFound(); // 404 

            return Ok(_mapper.Map<Product , ProductToReturnDto>(product));
        }
    }
}
