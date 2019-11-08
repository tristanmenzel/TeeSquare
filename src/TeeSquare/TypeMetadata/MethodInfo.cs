using System;
using TeeSquare.Writers;

namespace TeeSquare.TypeMetadata
{
    public interface IMethodInfo
    {
        bool IsStatic { get; }
        string Name { get; }
        string[] GenericTypeParams { get; }
        IIdentifierInfo[] Params { get; }
        ITypeReference ReturnType { get; }
        Action<ICodeWriter> WriteBody { get; }
    }

    class MethodInfo : IMethodConfigurator, IMethodInfo
    {
        private readonly ParamInfo _params;
        public bool IsStatic { get; private set; }
        public string Name { get; private set; }
        public string[] GenericTypeParams { get; }
        public IIdentifierInfo[] Params => _params.Params;
        public ITypeReference ReturnType { get; private set; }

        public MethodInfo(string name, params string[] genericTypeParams)
        {
            Name = name;
            GenericTypeParams = genericTypeParams;
            ReturnType = new TypeReference("void");
            _params = new ParamInfo();
            WriteBody = w => { };
        }

        public IMethodConfigurator WithReturnType(ITypeReference type)
        {
            ReturnType = type;
            return this;
        }

        public IMethodInfo Done()
        {
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
