using System;
using System.Reflection;
using TeeSquare.TypeMetadata;
using TeeSquare.Writers;
using MethodInfo = System.Reflection.MethodInfo;

namespace TeeSquare.Reflection
{
    public interface IReflectiveWriterOptions : ITypeScriptWriterOptions
    {
        /// <summary>
        /// A typeConverter instance used to determine the name of a dotnet Type in TypeScript
        /// </summary>
        TypeConverter TypeConverter { get; }

        /// <summary>
        /// An optional alternative typeConverter used to determine the name used for imports. If set,
        /// types will be imported with ImportName as Name
        /// </summary>
        TypeConverter ImportTypeConverter { get; }

        /// <summary>
        /// Specifies flags to check for when reflecting properties
        /// </summary>
        BindingFlags PropertyFlags { get; }

        /// <summary>
        /// Specifies flags to check for when reflecting fields
        /// </summary>
        BindingFlags FieldFlags { get; }

        /// <summary>
        /// Specifies flags to check for when reflecting methods
        /// </summary>
        BindingFlags MethodFlags { get; }

        /// <summary>
        /// Delegate to determine if methods should be reflected on a type. Defaults to false
        /// </summary>
        Func<Type, bool> ReflectMethods { get; }

        /// <summary>
        /// Delegate to determine if a single method should be reflected on a type. Defaults to true.
        /// Only checked if ReflectMethods returns true and the method matches MethodFlags
        /// </summary>
        Func<Type, MethodInfo, bool> ReflectMethod { get; }

        /// <summary>
        /// Strategy for rendering complex types. The default is to output them as interfaces but could
        /// be overridden to output classes
        /// </summary>
        WriteComplexType ComplexTypeStrategy { get; }

        /// <summary>
        /// Delegate to write a custom header at the top of the file. Default is a comment about the
        /// code being autogenerated
        /// </summary>
        WriteHeader WriteHeader { get; }

        /// <summary>
        /// Container for holding reflected and imported types.
        /// </summary>
        TypeCollection Types { get; }

        /// <summary>
        /// Determines whether enums are reflected to numeric types, or strings (using the member name as the value)
        /// </summary>
        public EnumValueTypeStrategy EnumValueTypeStrategy { get; }
    }
}
