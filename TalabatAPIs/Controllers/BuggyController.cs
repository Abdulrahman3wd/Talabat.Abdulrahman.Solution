using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Infrastrucure.Data;

namespace TalabatAPIs.Controllers
{
 
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _dbContext;

        public BuggyController(StoreContext dbContext)
        {
          _dbContext = dbContext;
        }
        [HttpGet("notfound")] // Get api/buggy/notfound
        public ActionResult GetNotFoundRequest()
        {
            var product = _dbContext.Products.Find(100);
            if (product is null)return NotFound();
            return Ok(product);
        }
        [HttpGet("servererror")]
        public ActionResult GetServerError() 
        {
            var product = _dbContext.Products.Find(100);
            var productToReturn = product.ToString(); // will Throw Exeption[NullRefernceType]
            return Ok(productToReturn);

        }
        [HttpGet("badRequest")]
        public ActionResult GetBadRequst()
        {
            return BadRequest();
        }
        [HttpGet("badRequest/{id}")]
        public IActionResult GetBadRequst(int id) // ValidationError
        {
            return Ok();

        }
    }
}
