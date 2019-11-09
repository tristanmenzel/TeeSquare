using System.ComponentModel;

namespace TeeSquare.Tests.Reflection.FakeDomain
{
    public enum Audience
    {
        Children,
        [Description("Teenagers")]
        Teenagers,
        [Description("Young Adults")]
        YoungAdults,
        [Description("Adults")]
        Adults
    }
}