using System.Collections.Generic;
using Mono.Cecil;

namespace Taiji
{
    public interface IAddAdapterMethod
    {
        void AddMethod(TypeDefinition adapterType,
                                       FieldReference targetField,
                                       MethodDefinition targetMethod,
                                       Dictionary<MethodReference, MethodReference> methodMap);
    }
}