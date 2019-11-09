using System.Collections.Generic;

namespace TeeSquare.TypeMetadata
{
    public interface IParamInfo
    {
        IIdentifierInfo[] Params { get; }
    }

    class ParamInfo : IParamConfigurator, IParamInfo
    {
        private readonly List<IIdentifierInfo> _params = new List<IIdentifierInfo>();
        public IIdentifierInfo[] Params => _params.ToArray();

        IParamConfigurator IParamConfigurator.Param(string name, ITypeReference type)
        {
            _params.Add(new IdentifierInfo(name, type));
            return this;
        }
    }
}
