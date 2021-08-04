
using System.ComponentModel;

namespace TeeSquare.Tests.Reflection.Enums
{
    public enum DescribedEnum
    {
        [Description("Negative One")]
        NegativeOne = -1,
        [Description("Zero")]
        Zero = 0,
        [Description("Positive One")]
        One = 1
    }
}
