using System;
using TeeSquare.Reflection;

namespace TeeSquare.Mobx
{
    public class TeeSquareMobx
    {
        public static void ConfigureMobxWriter(IReflectiveWriterOptions options) =>
            ConfigureMobxWriter(new MobxOptions())(options);

        public static Action<IReflectiveWriterOptions> ConfigureMobxWriter(IMobxOptions mobxOptions) =>
            (options) =>
            {
                var mobxWriterFactory = new MobxModelWriterFactory(mobxOptions);
                options.ComplexTypeStrategy = (writer, typeInfo, type) =>
                    writer.WriteSnippet(mobxWriterFactory.BuildModel(typeInfo));
                options.TypeConverter.AddStaticMappings(
                    (typeof(string), "types.string"),
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
                    (typeof(bool), "types.boolean"));
                options.Types.AddLiteralImport("mobx-state-tree", "types");
                options.Types.AddLiteralImport("mobx-state-tree", "Instance");
            };
    }
}
