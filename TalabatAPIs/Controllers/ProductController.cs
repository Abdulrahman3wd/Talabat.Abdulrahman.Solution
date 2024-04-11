using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace TalabatAPIs.Controllers
{

    public class ProductController : BaseApiController
    {
        public ProductController(IGenericRepository<Product> productRepository) 
        {

        }
    }
}
