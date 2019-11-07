using System;

namespace TeeSquare.WebApi.Reflection
{
    public class ParamInfo
    {
        public ParameterKind Kind { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}