using System;
using TeeSquare.Writers;

namespace TeeSquare.TypeMetadata
{
    public interface IMethodConfigurator
    {
        IMethodConfigurator Static();
        IMethodConfigurator WithParams(Action<IParamConfigurator> configureParams);
        IMethodConfigurator WithBody(Action<ICodeWriter> writeBody);
        IIdentifierInfo ReturnType { get; }
        IMethodConfigurator WithGenericTypeParams(params string[] genericTypeParams);
        IMethodConfigurator WithReturnType(string type, params string[] genericTypeParams);
        IMethodInfo Done();
    }
}
