using System;

namespace TeeSquare.TypeMetadata
{
    public interface ITypeConfigurer
    {
        void Property(string name, string type, params string[] genericTypeParams);

        IMethodConfigurator Method(string name);
    }
}