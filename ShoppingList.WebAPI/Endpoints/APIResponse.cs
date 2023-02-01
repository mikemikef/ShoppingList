using System.Net;

namespace ShoppingList.WebAPI.Endpoints
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>();
        }
        public bool IsSuccess { get; set; }
        public Object Result { get; set; } = null!;
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; }

    }
}
