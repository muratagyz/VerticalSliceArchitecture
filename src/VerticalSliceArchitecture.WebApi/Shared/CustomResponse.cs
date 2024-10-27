namespace VerticalSliceArchitecture.WebApi.Shared
{
    public class CustomResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public static CustomResponse<T> Success(T data)
        {
            return new CustomResponse<T> { Status = true, Data = data };
        }

        public static CustomResponse<T> Success(string message)
        {
            return new CustomResponse<T> { Status = true, Message = message };
        }

        public static CustomResponse<T> Success(T data, string message)
        {
            return new CustomResponse<T> { Status = true, Message = message, Data = data };
        }

        public static CustomResponse<T> Success(int statusCode)
        {
            return new CustomResponse<T> { Status = true };
        }

        public static CustomResponse<T> Fail(T data)
        {
            return new CustomResponse<T> { Status = false, Data = data };
        }

        public static CustomResponse<T> Fail(T data, string error)
        {
            return new CustomResponse<T> { Status = false, Errors = new List<string> { error }, Data = data };
        }

        public static CustomResponse<T> Fail(List<string> errors)
        {
            return new CustomResponse<T> { Status = false, Errors = errors };
        }

        public static CustomResponse<T> Fail(string error)
        {
            return new CustomResponse<T> { Status = false, Errors = new List<string> { error } };
        }

        public static CustomResponse<T> Fail(int statusCode)
        {
            return new CustomResponse<T> { Status = false };
        }
    }
}
