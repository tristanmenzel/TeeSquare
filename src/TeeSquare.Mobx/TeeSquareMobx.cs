using TeeSquare.Reflection;

namespace TeeSquare.Mobx
{
    public class TeeSquareMobx
    {
        public static void ConfigureMobxWriter(ReflectiveWriterOptions options)
        {
            var mobxWriterFactory = new MobxModelWriterFactory();
            options.ComplexTypeStrategy = (writer, typeInfo) =>
                writer.WriteSnippet(mobxWriterFactory.BuildModel(typeInfo));
            options.WriteHeader = (writer => { writer.WriteSnippet(mobxWriterFactory.BuildHeader()); });
        }
    }
}
