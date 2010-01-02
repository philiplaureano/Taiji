using System.Collections.Generic;
using Mono.Cecil;

namespace TaijiConsole
{
    public interface IAddAdapterMethod
    {
        void AddMethod(TypeDefinition adapterType,
                                       FieldReference targetField,
                                       MethodDefinition targetMethod,
                                       Dictionary<MethodReference, MethodReference> methodMap);
    }
}