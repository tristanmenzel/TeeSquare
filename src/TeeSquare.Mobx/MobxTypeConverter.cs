using System;
using TeeSquare.Reflection;

namespace TeeSquare.Mobx
{
    public class MobxTypeConverter : TypeConverter
    {
        public MobxTypeConverter(
        ) : base((typeof(string), "types.string"),
            (typeof(Guid), "types.string"),
            (typeof(Decimal), "types.number"),
            (typeof(byte), "types.integer"),
            (typeof(Int16), "types.integer"),
            (typeof(Int32), "types.integer"),
            (typeof(Int64), "types.integer"),
            (typeof(double), "types.number"),
            (typeof(Single), "types.number"),
            (typeof(DateTime), "types.Date"),
            (typeof(DateTimeOffset), "types.Date"),
            (typeof(bool), "types.boolean"))
        {
        }


        public override string TypeName(Type type)
        {
            return $"{base.TypeName(type)}Model";
        }
    }
}
