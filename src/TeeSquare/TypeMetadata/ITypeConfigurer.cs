namespace TeeSquare.Writers
{
    public interface ITypeConfigurer
    {
        void Property(string name, string type, params string[] genericTypeParams);

        IMethodInfo Method(string name, string returnType, params string[] returnTypeGenericTypeParams);
    }
}