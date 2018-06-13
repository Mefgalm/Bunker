namespace Bunker.Business.Interfaces.Models
{
    public class BaseResponse<T>
    {
        public bool   Ok     { get; set; }
        public string Info   { get; set; }
        public T      Result { get; set; }

        public static BaseResponse<T> Success() =>
            new BaseResponse<T>
            {
                Ok = true,
            };

        public static BaseResponse<T> Success(T result) =>
            new BaseResponse<T>
            {
                Ok     = true,
                Result = result,
            };

        public static BaseResponse<T> Fail() =>
            new BaseResponse<T>
            {
                Ok = false,
            };
        
        public static BaseResponse<T> Fail(string info) =>
            new BaseResponse<T>
            {
                Ok   = false,
                Info = info
            };
    }
}