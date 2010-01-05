using System.Collections.Generic;
using Mono.Cecil;

namespace Taiji
{
    public interface IMethodBuilder
    {
        MethodDefinition AddMethod(TypeDefinition targetType, 
            MethodReference originalMethod);
    }
}