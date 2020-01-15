using System.Collections;
using System.Collections.Generic;

namespace TeeSquare.Mobx
{
    public interface IMobxOptions
    {
        bool EmitInstanceType { get; }
        MobxTypeNamer InstanceTypeName { get; }
        string OptionalType { get; set; }
        string CollectionType { get; set; }
        string EnumType { get; set; }
    }

    public delegate string MobxTypeNamer(string typeName);

    public class MobxOptions : IMobxOptions
    {
        public string OptionalType { get; set; } = "types.maybe({0})";
        public string CollectionType { get; set; } = "types.array({0})";
        public string EnumType { get; set; } = "types.frozen<{0}>()";
        public bool EmitInstanceType { get; set; } = true;

        public MobxTypeNamer InstanceTypeName { get; set; } = name => name.EndsWith("Model")
            ? $"{name.Substring(0, name.Length - 5)}Instance"
            : $"{name}Instance";
    }
}
