namespace RPS.myProjects.Infrastructure.Util
{
    public class SuccessResult<TSuccess>: Result<TSuccess>
    {
        public const bool IsSuccess = true;

        public TSuccess Value { get; }

        public SuccessResult(TSuccess val)
        {
            Value = val;
        }
    }
}
