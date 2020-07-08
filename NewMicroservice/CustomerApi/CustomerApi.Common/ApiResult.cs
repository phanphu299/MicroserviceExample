
namespace CustomerApi.Common
{
    public class ApiResult<T>
    {
        public T Payload { get; }
        public string FailureReason { get; }
        public bool IsSuccess => FailureReason == null;

        //builder
        private ApiResult(string reason) => FailureReason = reason;
        private ApiResult(T payload) => Payload = payload;

        //methods
        public static ApiResult<T> Fail(string reason) => new ApiResult<T>(reason);
        public static ApiResult<T> Success(T payload) => new ApiResult<T>(payload);
        public static implicit operator bool(ApiResult<T> result) => result.IsSuccess;
    }
}
