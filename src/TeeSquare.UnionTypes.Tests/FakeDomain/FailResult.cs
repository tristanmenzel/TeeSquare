namespace RPS.myProjects.Infrastructure.Util
{
    public class FailResult<TSuccess>: Result<TSuccess>
    {
        public const bool IsSuccess = false;
        public string Message { get; }

        public FailResult(string message)
        {
            Message = message;
        }
    }
}
