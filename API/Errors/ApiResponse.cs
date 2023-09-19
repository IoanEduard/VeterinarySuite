
namespace API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }
        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch {
                400 => "bad request",
                401 => "unauthorized",
                404 => "resource not found",
                409 => "there is a conflict",
                500 => "internal error",
                _ => "unknown error"
            };
        }
    }
}