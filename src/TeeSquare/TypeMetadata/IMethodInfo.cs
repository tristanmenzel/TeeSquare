using System;
using TeeSquare.Writers;

namespace TeeSquare.TypeMetadata
{
    public interface IMethodInfo
    {
        IMethodInfo Static();
        IMethodInfo WithParams(Action<IParamsInfo> configureParams);
        IMethodInfo WithBody(Action<ICodeWriter> writeBody);
        IIdentifierInfo ReturnType { get; }
        IMethodInfo WithGenericTypeParams(params string[] genericTypeParams);
        IMethodInfo WithReturnType(string type, params string[] genericTypeParams);
    }
}