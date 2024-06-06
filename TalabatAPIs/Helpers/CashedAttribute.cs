using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services.Contract;

namespace TalabatAPIs.Helpers
{
    public class CashedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSecond;

        public CashedAttribute( int timeToLiveInSecond)
        {
            _timeToLiveInSecond = timeToLiveInSecond;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
          var responseCasheServices =  context.HttpContext.RequestServices.GetRequiredService<IResponseCasheServices>();
            // Ask CLR for Creating object From Response Cahse Services (Explicitly)
            var casheKey = GenerateCasheKeyFromRequest(context.HttpContext.Request);

            var response = await responseCasheServices.GetCashedResponseAsync(casheKey);

            if (!string.IsNullOrEmpty(response))
            {
                var result = new ContentResult
                {
                    Content = response, 
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = result;
                return;
            } // Response is Not Cashed

            var executedActionContext =  await next.Invoke(); // Will Execute The Next Action Filter Or Action it Slef
            if (executedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null) 
            {
                await responseCasheServices.CasheResponseAsync(casheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSecond));
            }
        }

        private string GenerateCasheKeyFromRequest(HttpRequest request)
        {
            // {{url}}api/products?pageIndex=1&pageSize=5&sort=name
            var keyBuilder =  new StringBuilder();
            keyBuilder.Append(request.Path);


            // pageIndex=1
            // pageSize=5
            // sort=name

            foreach (var (key , value) in request.Query.OrderBy(X=>X.Key))
            {

                keyBuilder.Append($"|{key}-{value}");
                // api/products|pageIndex-1
                // api/products|pageIndex-1|pageSize=5
                // api/products|pageIndex-1|pageSize=5|sort=name


            }
            return keyBuilder.ToString();
        }
    }
}
