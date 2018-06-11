using System;

namespace TeeSquare.TypeMetadata
{
    public interface ITypeConfigurer
    {
        void Property(string name, string type, params string[] genericTypeParams);

        [Obsolete]
        IMethodInfo Method(string name, string returnType, params string[] returnTypeGenericTypeParams);
        IMethodInfo Method(string name);
    }
}