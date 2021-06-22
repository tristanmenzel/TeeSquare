using TeeSquare.Reflection;
using TeeSquare.TypeMetadata;

namespace TeeSquare.WebApi.Reflection
{
    public interface IRouteReflectorOptions : IReflectiveWriterOptions
    {
        /// <summary>
        /// The default route pattern to use in the absence of route attributes.
        ///
        /// eg. {controller=Home}/{action=Index}/{id?}
        /// </summary>
        string DefaultRoute { get; set; }

        /// <summary>
        /// Determines the name of the request factory for a given route. Default is all routes into
        /// a factory named 'RequestFactory'.
        /// </summary>
        FactoryNameStrategy FactoryNameStrategy { get; set; }

        /// <summary>
        /// The strategy to use to determine the return type of an api method. Default is to use
        /// the return value.
        /// </summary>
        GetApiReturnType GetApiReturnTypeStrategy { get; set; }

        /// <summary>
        /// The strategy to use to determine the route of an api method. Default is to use
        /// Route attributes.
        /// </summary>
        BuildRoute BuildRouteStrategy { get; set; }

        /// <summary>
        /// The strategy to use to determine the name of the route on the request factory. Default is to use
        /// the pascal cased route string.
        /// </summary>
        NameRoute NameRouteStrategy { get; set; }

        /// <summary>
        /// The option for obtaining request helper types. Default is to emit them with each file.
        /// They can alternatively be imported.
        /// </summary>
        RequestHelperTypeOptions RequestHelperTypeOption { get; set; }

        /// <summary>
        /// The strategy to use to determine the http method of an api method. Should return a RequestFactory
        /// for that request type too. Default strategy looks for HttpGet/Put etc attributes
        /// </summary>
        GetHttpMethodAndRequestFactory GetHttpMethodAndRequestFactoryStrategy { get; set; }

        /// <summary>
        /// The strategy to use to determine the kind of a parameter (ie. query string / route / body )
        /// </summary>
        GetParameterKind GetParameterKindStrategy { get; set; }

        /// <summary>
        /// The <see cref="TypeReference"/> to use to represent an empty body type (ie undefined, null, any)
        /// </summary>
        TypeReference EmptyRequestBodyType { get; set; }

        /// <summary>
        /// The name of the variable used inside request factory methods for storing the query data. Can be
        /// renamed from the default to avoid name clashes
        /// </summary>
        string QueryVariableName { get; set; }

        /// <summary>
        /// A prefix prepended to all route urls
        /// </summary>
        string RoutePrefix { get; set; }
    }
}
