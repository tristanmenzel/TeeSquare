using System;
using TeeSquare.Writers;

namespace TeeSquare.TypeMetadata
{
    public interface IMethodInfo
    {
        bool IsStatic { get; }
        IIdentifierInfo Id { get; }
        IIdentifierInfo[] Params { get; }
        IIdentifierInfo ReturnType { get; }
        Action<ICodeWriter> WriteBody { get; }
    }

    class MethodInfo : IMethodConfigurator, IMethodInfo
    {
        private readonly ParamInfo _params;
        public bool IsStatic { get; private set; }
        public IIdentifierInfo Id { get; private set; }
        public IIdentifierInfo[] Params => _params.Params;
        public IIdentifierInfo ReturnType { get; private set; }

        public MethodInfo(string name)
        {
            Id = new IdentifierInfo(name, null);
            ReturnType = new IdentifierInfo(null, "void");
            _params = new ParamInfo();
        }

        public IMethodConfigurator WithGenericTypeParams(params string[] genericTypeParams)
        {
            Id = new IdentifierInfo(Id.Name, null, genericTypeParams);
            return this;
        }

        public IMethodConfigurator WithReturnType(string type, params string[] genericTypeParams)
        {
            ReturnType = new IdentifierInfo(null, type, genericTypeParams);
            return this;
        }

        public IMethodConfigurator Static()
        {
            IsStatic = true;
            return this;
        }

        public IMethodConfigurator WithParams(Action<IParamConfigurator> configureParams)
        {
            configureParams(_params);
            return this;
        }

        public IMethodConfigurator WithBody(Action<ICodeWriter> writeBody)
        {
            WriteBody = writeBody;
            return this;
        }

        public Action<ICodeWriter> WriteBody { get; private set; }
    }
}