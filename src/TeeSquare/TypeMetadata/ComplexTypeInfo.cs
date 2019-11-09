using System;
using System.Collections.Generic;
using System.Linq;

namespace TeeSquare.TypeMetadata
{
    public interface IComplexTypeInfo
    {
        ITypeReference TypeReference { get; }
        bool IsAbstract { get; }
        ITypeReference[] GenericTypeParams { get; }
        string Name { get; }
        IPropertyInfo[] Properties { get; }
        IMethodInfo[] Methods { get; }
    }

    class ComplexTypeInfo : IComplexTypeConfigurator, IComplexTypeInfo
    {
        public ComplexTypeInfo(string name, ITypeReference[] genericTypeParams = null)
        {
            Name = name;
            GenericTypeParams = genericTypeParams ?? Array.Empty<ITypeReference>();
        }

        public ITypeReference[] GenericTypeParams { get; }

        public Type OriginalType { get; }
        public string Name { get;  }

        public ITypeReference TypeReference => new TypeReference(Name, GenericTypeParams);
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


        public void AddProperty(string name, ITypeReference typeReference)
        {
            _properties.Add(new PropertyInfo(name,  typeReference));
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
