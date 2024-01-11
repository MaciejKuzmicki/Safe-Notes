using System.Net;

namespace Safe_Notes_Api.Models;

public class ServiceResponse<T>
{
    public T Data { get; set; }
    
    public bool Success { get; set; }
    
    public string Message { get; set; }
    
    public HttpStatusCode StatusCode { get; set; }
}

