using System.ComponentModel;
using System.Reflection;

namespace TeeSquare.UnionTypes.Tests.FakeDomain
{
    public enum FieldType
    {
        String,
        Number,
    }

    [UnionType(typeof(NumericField), typeof(StringField))]
    public class Field
    {

    }

    public class NumericField : Field
    {
        [AsConst(FieldType.Number)]
        public FieldType Type => FieldType.Number;

        public int Value { get; set; }
    }

    public class StringField : Field
    {
        [AsConst(FieldType.String)]
        public FieldType Type => FieldType.String;

        public string Value { get; set; }
    }
}
