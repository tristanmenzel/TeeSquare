using System.Collections.Generic;

namespace TeeSquare.TypeMetadata
{
    class ParamsInfo : IParamsInfo
    {
        private readonly List<IIdentifierInfo> _params = new List<IIdentifierInfo>();
        public IIdentifierInfo[] Params => _params.ToArray();

        public IParamsInfo Param(string name, string type, params string[] genericTypeParams)
        {
            _params.Add(new IdentifierInfo(name, type, genericTypeParams));
            return this;
        }
    }
}