using System;
using System.Collections.Generic;
using System.Linq;

namespace TeeSquare.TypeMetadata
{
    public interface IEnumInfo
    {
        string Name { get; }
        EnumValueType ValueType { get; }
        IEnumValueInfo[] Values { get; }
    }

    public enum EnumValueType
    {
        String,
        Number
    }

    class EnumInfo : IEnumConfigurator, IEnumInfo
    {
        public string Name { get; }
        public EnumValueType ValueType { get; }

        public EnumInfo(string name, EnumValueType valueType)
        {
            Name = name;
            ValueType = valueType;
        }

        private readonly List<EnumValueInfo> _values = new List<EnumValueInfo>();

        public IEnumValueInfo[] Values => _values.Cast<IEnumValueInfo>().ToArray();

        public IEnumConfigurator Configure(Action<IEnumConfigurator> configure)
        {
            configure(this);
            return this;
        }

        void IEnumConfigurator.AddValue(string name, int value, string description)
        {
            _values.Add(new EnumValueInfo(name, value, description));
        }

        void IEnumConfigurator.AddValue(string name, string value, string description)
        {
            _values.Add(new EnumValueInfo(name, value, description));
        }

        public IEnumInfo Done()
        {
            return this;
        }
    }
}
