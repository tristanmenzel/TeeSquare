using System;

namespace TeeSquare.Writers
{
    public interface IMethodInfo
    {
        IMethodInfo Static();
        IMethodInfo WithParams(Action<IParamsInfo> configureParams);
        IMethodInfo WithBody(Action<ICodeWriter> writeBody);
    }
}