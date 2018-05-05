using System;

namespace TeeSquare.Writers
{
    class MethodInfo : IMethodInfo
    {
        private readonly ParamsInfo _params;
        public bool IsStatic { get; private set; }
        public IIdentifierInfo Id { get; }
        public IIdentifierInfo[] Params => _params.Params;

        public MethodInfo(string name, string returnType, params string[] returnTypeGenericTypeParams)
        {
            Id = new IdentifierInfo(name, returnType, returnTypeGenericTypeParams);
            _params = new ParamsInfo();
        }

        public IMethodInfo Static()
        {
            IsStatic = true;
            return this;
        }

        public IMethodInfo WithParams(Action<IParamsInfo> configureParams)
        {
            configureParams(_params);
            return this;
        }

        public IMethodInfo WithBody(Action<ICodeWriter> writeBody)
        {
            WriteBody = writeBody;
            return this;
        }

        public Action<ICodeWriter> WriteBody { get; private set; }
    }
}