using TeeSquare.UnionTypes;

namespace RPS.myProjects.Infrastructure.Util
{
    public class SuccessResult<TSuccess>: Result<TSuccess>
    {
        [AsConst(true)]
        public  bool IsSuccess => true;

        public TSuccess Value { get; }

        public SuccessResult(TSuccess val)
        {
            Value = val;
        }
    }
}
