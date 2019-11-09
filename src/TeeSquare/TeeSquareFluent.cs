using TeeSquare.Reflection;

namespace TeeSquare
{
    public static class TeeSquareFluent
    {
        public static ReflectiveWriterFluent ReflectiveWriter()
        {
            return new ReflectiveWriterFluent();
        }
    }
}