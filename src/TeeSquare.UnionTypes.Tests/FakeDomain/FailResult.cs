namespace RPS.myProjects.Infrastructure.Util
{
    public class FailResult<TSuccess>: Result<TSuccess>
    {
        public string Message { get; }

        public FailResult(string message)
        {
            Message = message;
            IsSuccess = false;
        }
    }
}