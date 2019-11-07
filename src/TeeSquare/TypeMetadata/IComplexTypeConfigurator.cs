using System;

namespace TeeSquare.TypeMetadata
{
    public interface IComplexTypeConfigurator
    {
        void AddProperty(string name, string type, params string[] genericTypeParams);

        IMethodConfigurator AddMethod(string name);

        IComplexTypeConfigurator Configure(Action<IComplexTypeConfigurator> configure);

        IComplexTypeInfo Done();
        IComplexTypeConfigurator MakeAbstract(bool isAbstract = true);
    }
}
