using System.Collections.Generic;
using Mono.Cecil;

namespace Taiji
{
    public interface ITypeBuilder
    {
        TypeDefinition CreateType(string typeName, 
            string namespaceName, 
            Dictionary<MethodReference, MethodReference> methodMap);
    }
}