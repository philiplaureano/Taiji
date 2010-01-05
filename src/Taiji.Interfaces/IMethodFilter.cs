using System.Collections.Generic;
using Mono.Cecil;

namespace Taiji
{
    public interface IMethodFilter
    {
        IEnumerable<MethodReference> GetMethods(IEnumerable<MethodDefinition> targetMethods,
                                                                TypeDefinition targetDependency);
    }
}