using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using TeeSquare.Writers;

namespace TeeSquare.TypeMetadata
{
    public interface ITypeInfo
    {
        bool IsAbstract { get; }
        string[] GenericTypeParams { get; }
        string Name { get; }
        IPropertyInfo[] Properties { get; }
        IMethodInfo[] Methods { get; }
    }

    class TypeInfo : ITypeConfigurer, ITypeInfo
    {
        public TypeInfo(string name, string[] genericTypeParams)
        {
            Name = name;
            GenericTypeParams = genericTypeParams;
        }

        public string[] GenericTypeParams { get; }

        public string Name { get;  }
        
        public bool IsAbstract { get; set; }

        private readonly List<PropertyInfo> _properties = new List<PropertyInfo>();
        private readonly List<MethodInfo> _methods = new List<MethodInfo>();

        public IPropertyInfo[] Properties => _properties.Cast<IPropertyInfo>().ToArray();
        public IMethodInfo[] Methods => _methods.Cast<IMethodInfo>().ToArray();

        public void Property(string name, string type, string[] genericTypeParams)
        {
            _properties.Add(new PropertyInfo(name, type, genericTypeParams));
        }

        public IMethodConfigurator Method(string name)
        {
            var methodInfo = new MethodInfo(name);
            _methods.Add(methodInfo);
            return methodInfo;
        }
    }
}