using System.Collections.Generic;
using System.Linq;

namespace TeeSquare.TypeMetadata
{
    public interface IEnumInfo
    {
        string Name { get; }
        IEnumValueInfo[] Values { get; }
    }

    class EnumInfo : IEnumConfigurator, IEnumInfo
    {
        public string Name { get; }

        public EnumInfo(string name)
        {
            Name = name;
        }
        
        private readonly List<EnumValueInfo> _values = new List<EnumValueInfo>();
        
        public IEnumValueInfo[] Values => _values.Cast<IEnumValueInfo>().ToArray();

        void IEnumConfigurator.Value(string name, int value, string description)
        {
            _values.Add(new EnumValueInfo(name, value, description));
        }

        void IEnumConfigurator.Value(string name, string value, string description)
        {
            _values.Add(new EnumValueInfo(name, value, description));
        }
    }
}