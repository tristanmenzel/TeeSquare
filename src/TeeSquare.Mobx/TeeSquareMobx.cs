using System;
using TeeSquare.Reflection;

namespace TeeSquare.Mobx
{
    public class TeeSquareMobx
    {
        public static void ConfigureMobxWriter(IReflectiveWriterOptions options) =>
            ConfigureMobxWriter(new MobxOptions())(options);

        public static Action<IReflectiveWriterOptions> ConfigureMobxWriter(IMobxOptions mobxOptions, TypeConverter typeConverter = null) =>
            (options) =>
            {
                var mobxWriterFactory = new MobxModelWriterFactory(mobxOptions);
                options.ComplexTypeStrategy = (writer, typeInfo, type) =>
                    writer.WriteSnippet(mobxWriterFactory.BuildModel(typeInfo));
                options.TypeConverter =  typeConverter?? new MobxTypeConverter();
                options.Types.AddLiteralImport("mobx-state-tree", "types");
                options.Types.AddLiteralImport("mobx-state-tree", "Instance");
            };
    }
}
