using System;
using System.Reflection;
using TeeSquare.Writers;

namespace TeeSquare.Reflection
{
    public interface IReflectiveWriterOptions: ITypeScriptWriterOptions
    {
        Namer Namer { get;  }
        BindingFlags PropertyFlags { get;  }
        BindingFlags MethodFlags { get;  }
        Func<Type, bool> ReflectMethods { get;  }
        WriteComplexType ComplexTypeStrategy { get;  }
        WriteHeader WriteHeader { get;  }
        DiscriminatorPropertyPredicate DiscriminatorPropertyPredicate { get;  }
        DiscriminatorPropertyValueProvider DiscriminatorPropertyValueProvider { get;  }
    }
}
