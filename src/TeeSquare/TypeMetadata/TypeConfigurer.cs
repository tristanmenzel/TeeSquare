using System;
using System.Collections.Generic;
using TeeSquare.Writers;

namespace TeeSquare.TypeMetadata
{
    class TypeConfigurer : ITypeConfigurer
    {
        private readonly List<PropertyInfo> _properties = new List<PropertyInfo>();
        private readonly List<MethodInfo> _methods = new List<MethodInfo>();

        public PropertyInfo[] Properties => _properties.ToArray();
        public MethodInfo[] Methods => _methods.ToArray();

        public void Property(string name, string type, string[] genericTypeParams)
        {
            _properties.Add(new PropertyInfo(name, type, genericTypeParams));
        }

        [Obsolete]
        public IMethodInfo Method(string name, string returnType, params string[] returnTypeGenericTypeParams)
        {
            var methodInfo = new MethodInfo(name);
            methodInfo.WithReturnType(returnType, returnTypeGenericTypeParams);
            _methods.Add(methodInfo);
            return methodInfo;
        }

        public IMethodInfo Method(string name)
        {
            var methodInfo = new MethodInfo(name);
            _methods.Add(methodInfo);
            return methodInfo;
        }
    }
}