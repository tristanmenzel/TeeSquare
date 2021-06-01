using System;
using System.Linq;

namespace TeeSquare.WebApi.Reflection
{
    public class ParamInfo
    {
        public ParameterKind Kind { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public DestructuredPropertyInfo[] DestructuredProperties { get; set; }

        public string NameOrDestructureExpression
        {
            get
            {
                if (DestructuredProperties.Any())
                {
                    var propsFormatted = String.Join(", ", DestructuredProperties
                            .Select(p => p.DestructureExpression));
                    return $"{{ {propsFormatted} }}";
                }

                return Name;
            }
        }
    }
}
