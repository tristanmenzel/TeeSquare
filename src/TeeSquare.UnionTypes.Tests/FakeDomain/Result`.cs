using System;
using TeeSquare.UnionTypes;

namespace RPS.myProjects.Infrastructure.Util
{
    [UnionType(typeof(SuccessResult<>), typeof(FailResult<>))]
    public class Result<TSuccess> : Result
    {
        protected Result()
        {
        }

        public static implicit operator Result<TSuccess>(FailResult<Unbound> success)
        {
            return new FailResult<TSuccess>(success.Message);
        }
    }
}
