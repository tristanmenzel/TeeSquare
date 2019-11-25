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

            options.Types.AddLiteralImport("mobx-state-tree", "types");
            options.Types.AddLiteralImport("mobx-state-tree", "Instance");
        }
    }
}
