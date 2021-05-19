using System;

namespace TeeSquare.WebApi.Reflection
{
    public delegate RequestInfo RequestFactory(string factoryName, string name, string path, Type responseType,
        ParamInfo[] requestParams);
}
