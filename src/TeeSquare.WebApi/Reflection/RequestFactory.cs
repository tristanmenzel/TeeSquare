using System;

namespace TeeSquare.WebApi.Reflection
{
    public delegate RequestInfo RequestFactory(string name, string path, Type responseType,
        ParamInfo[] requestParams);
}