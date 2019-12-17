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
        bool EmitInstanceType { get; }
        MobxTypeNamer ModelName { get; }
        MobxTypeNamer InstanceTypeName { get; }
    }

    public delegate string MobxTypeNamer(string typeName);

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
        public bool EmitInstanceType { get; set; } = true;
        public MobxTypeNamer ModelName { get; set; } = name => $"{name}Model";
        public MobxTypeNamer InstanceTypeName { get; set; } = name => $"{name}Instance";
    }
}
