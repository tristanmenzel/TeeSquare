using System.Collections.Generic;

namespace TeeSquare.TypeMetadata
{
    class EnumTypeConfigurator : IEnumTypeConfigurator
    {
        private readonly List<ValueInfo> _values = new List<ValueInfo>();

        public ValueInfo[] Values => _values.ToArray();

        public void Value(string name, int value, string description = null)
        {
            _values.Add(new ValueInfo(name, value, description));
        }

        public void Value(string name, string value, string description = null)
        {
            _values.Add(new ValueInfo(name, value, description));
        }
    }
}