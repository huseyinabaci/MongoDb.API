using System.Net;

namespace MongoDb.API.DTOs
{
    public class ResponseDto<T>
    {
        public T? Result { get; set; }
        public List<string>? Errors { get; set; }
        public bool IsSuccessful { get; set; }
        public HttpStatusCode Status { get; set; }


        // Static Factory Methods
        public static ResponseDto<T> Success(T result, HttpStatusCode status)
        {
            return new ResponseDto<T> { Result = result, Status = status, IsSuccessful = true };
        }

        public static ResponseDto<T> Fail(List<string> errors, HttpStatusCode status)
        {
            return new ResponseDto<T> { Errors = errors, Status = status, IsSuccessful = false };
        }

        public static ResponseDto<T> Fail(string error, HttpStatusCode status)
        {
            return new ResponseDto<T> { Errors = new List<string>() { error }, Status = status , IsSuccessful = false };
        }
    }
}
