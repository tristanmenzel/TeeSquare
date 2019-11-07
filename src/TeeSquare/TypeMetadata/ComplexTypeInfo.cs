using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using TeeSquare.Writers;

namespace TeeSquare.TypeMetadata
{
    public interface IComplexTypeInfo
    {
        bool IsAbstract { get; }
        string[] GenericTypeParams { get; }
        string Name { get; }
        IPropertyInfo[] Properties { get; }
        IMethodInfo[] Methods { get; }
    }

    class ComplexTypeInfo : IComplexTypeConfigurator, IComplexTypeInfo
    {
        public ComplexTypeInfo(string name, string[] genericTypeParams = null)
        {
            Name = name;
            GenericTypeParams = genericTypeParams ?? Array.Empty<string>();
        }

        public string[] GenericTypeParams { get; }

        public Type OriginalType { get; }
        public string Name { get;  }

        public bool IsAbstract { get; set; }

        private readonly List<PropertyInfo> _properties = new List<PropertyInfo>();
        private readonly List<MethodInfo> _methods = new List<MethodInfo>();

        public IPropertyInfo[] Properties => _properties.Cast<IPropertyInfo>().ToArray();
        public IMethodInfo[] Methods => _methods.Cast<IMethodInfo>().ToArray();

        public IComplexTypeConfigurator MakeAbstract(bool isAbstract = true)
        {
            IsAbstract = isAbstract;
            return this;
        }


        public void AddProperty(string name, string type, string[] genericTypeParams)
        {
            _properties.Add(new PropertyInfo(name, type, genericTypeParams));
        }

        public IMethodConfigurator AddMethod(string name)
        {
            var methodInfo = new MethodInfo(name);
            _methods.Add(methodInfo);
            return methodInfo;
        }

        public IComplexTypeConfigurator Configure(Action<IComplexTypeConfigurator> configure)
        {
            configure(this);
            return this;
        }

        public IComplexTypeInfo Done()
        {
            return this;
        }
    }
}
