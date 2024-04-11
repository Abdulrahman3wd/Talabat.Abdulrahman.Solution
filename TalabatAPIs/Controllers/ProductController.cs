using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace TalabatAPIs.Controllers
{

    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepository;

        public ProductController(IGenericRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }
        // /api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        //api/products/10
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product is null)
                return NotFound(); // 404 

            return Ok(product);
        }
    }
}
