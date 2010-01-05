using System.Collections.Generic;
using Mono.Cecil;

namespace Taiji
{
    public interface IAdapterBuilder
    {
        MethodDefinition CreateAdapterConstructor(Dictionary<MethodReference, MethodReference> methodMap);
    }
}