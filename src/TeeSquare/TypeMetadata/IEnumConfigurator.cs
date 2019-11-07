using System;

namespace TeeSquare.TypeMetadata
{
    public interface IEnumConfigurator
    {
        void AddValue(string name, int value, string description = null);
        void AddValue(string name, string value, string description = null);
        IEnumInfo Done();
        IEnumConfigurator Configure(Action<IEnumConfigurator> configure);
    }
}
