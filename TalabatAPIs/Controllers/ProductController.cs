using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpecs;
using TalabatAPIs.DTOs;
using TalabatAPIs.Errors;
using TalabatAPIs.Helpers;

namespace TalabatAPIs.Controllers
{
    [Authorize]

    public class ProductController : BaseApiController
    {
		private readonly IProductServices _productServices;

		///private readonly IGenericRepository<Product> _productRepository;
		///private readonly IGenericRepository<ProductBrand> _bransRepository;
		///private readonly IGenericRepository<ProductCategory> _categoryRepository;
		private readonly IMapper _mapper;

        public ProductController(
            IProductServices productServices,
            IMapper mapper)
        {
			_productServices = productServices;
			///_productRepository = productRepository;
			///_bransRepository = bransRepository;
			///_categoryRepository = categoryRepository;
			_mapper = mapper;
        }
        [CashedAttribute(600)]
        // /api/Products
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecificationsParams specParams)
        {

            var products = await _productServices.GetProductsAsync(specParams); 
            var count = await _productServices.GetCountAsync(specParams);
			var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

			return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex ,specParams.PageSize, count, data));
        }



        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound )]
        //api/products/10
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            
            var product = await _productServices.GetProductAsync(id);
            if (product is null)
                return NotFound(new ApiResponse(404)); // 404 

            return Ok(_mapper.Map<Product , ProductToReturnDto>( product));
        }
        [HttpGet("brands")] // Get: /api/products/brands 

        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> getBrands ()
        {  
            var brands = await _productServices.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("catigories")] // Get: /api/catigories/brands 
        
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategoris()
        {
            var Categories = await _productServices.GetCatigoriesAsync();
            return Ok(Categories);
        } 
    }
}
