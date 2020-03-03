using System;

namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public class Audit
    {
        public readonly Guid Id;
        public readonly DateTimeOffset TimeStamp;
        public readonly string Action;

        public Audit(Guid id, DateTimeOffset timeStamp, string action)
        {
            Id = id;
            TimeStamp = timeStamp;
            Action = action;
        }
    }
}
