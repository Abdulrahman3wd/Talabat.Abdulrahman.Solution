
namespace TalabatAPIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statusCode , string? message = null)
        {
            StatusCode = statusCode ;
            Message = message ?? GetDefultMessageForStatusCode(statusCode);
        }

        private string? GetDefultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad Request , You have made",
                401 => "Authorized , You Are Not",
                404 => "Resouce was not Found",
                500 => "Errors are the path to the dark side , Errors lead to anger , anger leads to hete",
                _ => null
            };
        }
    }
}
