using System;
using TeeSquare.Writers;

namespace TeeSquare.TypeMetadata
{
    public interface IMethodConfigurator
    {
        IMethodConfigurator Static();
        IMethodConfigurator WithParams(Action<IParamConfigurator> configureParams);
        IMethodConfigurator WithBody(Action<ICodeWriter> writeBody);
        IMethodConfigurator WithReturnType(ITypeReference typeReference);
        IMethodInfo Done();
    }
}
