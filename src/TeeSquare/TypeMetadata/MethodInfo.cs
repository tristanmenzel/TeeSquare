using System;
using TeeSquare.Writers;

namespace TeeSquare.TypeMetadata
{
    class MethodInfo : IMethodInfo
    {
        private readonly ParamsInfo _params;
        public bool IsStatic { get; private set; }
        public IIdentifierInfo Id { get; private set; }
        public IIdentifierInfo[] Params => _params.Params;
        public IIdentifierInfo ReturnType { get; private set; }

        public MethodInfo(string name)
        {
            Id = new IdentifierInfo(name, null);
            ReturnType = new IdentifierInfo(null, "void");
            _params = new ParamsInfo();
        }

        public IMethodInfo WithGenericTypeParams(params string[] genericTypeParams)
        {
            Id = new IdentifierInfo(Id.Name, null, genericTypeParams);
            return this;
        }

        public IMethodInfo WithReturnType(string type, params string[] genericTypeParams)
        {
            ReturnType = new IdentifierInfo(null, type, genericTypeParams);
            return this;
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