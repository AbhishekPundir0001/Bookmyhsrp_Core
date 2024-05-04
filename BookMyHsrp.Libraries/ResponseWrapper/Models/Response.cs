namespace BookMyHsrp.Libraries.ResponseWrapper.Models;

public class Response<T>
{
    public bool Error { get; set; }
    public string Message { get; set; }   
    public dynamic Token { get; set; }
    public T Data { get; set; }
   
    public Response() {}

    public Response(T data, bool error = false, string message = null, dynamic tokens=null)
    {
        Data = data;
        Error = error;
        Message = message;
        Token = tokens;
        
    }
   

}