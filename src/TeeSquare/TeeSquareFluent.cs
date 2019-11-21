using TeeSquare.Reflection;
using TeeSquare.Writers;

namespace TeeSquare
{
    public static class TeeSquareFluent
    {
        public static ReflectiveWriterFluent ReflectiveWriter()
        {
            return new ReflectiveWriterFluent();
        }

        public static TypeScriptWriterFluent StandardWriter()
        {
            return new TypeScriptWriterFluent();
        }
    }
}
