using System.Collections.Generic;
using Mono.Cecil;

namespace TaijiConsole
{
    public interface IMethodBuilder
    {
        MethodDefinition AddMethod(TypeDefinition targetType, 
            MethodReference originalMethod);
    }
}