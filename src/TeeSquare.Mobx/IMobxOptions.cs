using System.Collections;
using System.Collections.Generic;

namespace TeeSquare.Mobx
{
    public interface IMobxOptions
    {
        string OptionalType { get; }
        string DateType { get; }
        string IntegerType { get; }
        string NumericIdentityType { get; }
        string StringIdentityType { get; }
        string DecimalType { get; }
        string StringType { get; }
        string BooleanType { get; }
    }

    public class MobxOptions : IMobxOptions
    {
        public string OptionalType { get; set; } = "types.maybe";
        public string DateType { get; set; } = "types.Date";
        public string IntegerType { get; set; } = "types.integer";
        public string NumericIdentityType { get; set; } = "types.identifierNumber";
        public string StringIdentityType { get; set; } = "types.identifier";
        public string DecimalType { get; set; } = "types.number";
        public string StringType { get; set; } = "types.string";
        public string BooleanType { get; set; } = "types.boolean";
    }
}
