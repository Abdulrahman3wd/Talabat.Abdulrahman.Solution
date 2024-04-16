using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Infrastrucure.Data;
using TalabatAPIs.Errors;

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
            if (product is null)
                return NotFound(new ApiResponse(404));
            return Ok(product);
        }
        [HttpGet("servererror")]
        public ActionResult GetServerError() 
        {
            var product = _dbContext.Products.Find(100);
            var productToReturn = product?.ToString(); // will Throw Exeption[NullRefernceType]
            return Ok(productToReturn);

        }
        [HttpGet("badrequest")]
        public ActionResult GetBadRequst()
        {
            return BadRequest(new ApiResponse(400));
        }
        [HttpGet("badRequest/{id}")]
        public IActionResult GetBadRequst(int id) // ValidationError
        {
            return Ok();

        }
        [HttpGet("unAuthorized")]
        public ActionResult GetUnAuthorizedError()
        {
            return Unauthorized(new ApiResponse(401));
        }
    }
}
