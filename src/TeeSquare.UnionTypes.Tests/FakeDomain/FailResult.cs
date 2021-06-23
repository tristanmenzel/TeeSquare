using TeeSquare.UnionTypes;

namespace RPS.myProjects.Infrastructure.Util
{
    public class FailResult<TSuccess>: Result<TSuccess>
    {
        [AsConst(false)]
        public bool IsSuccess => false;
        public string Message { get; }

        public FailResult(string message)
        {
            Message = message;
        }
    }
}
