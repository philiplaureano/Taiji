using System.Collections.Generic;
using Mono.Cecil;

namespace TaijiConsole
{
    public interface IAdapterBuilder
    {
        MethodDefinition CreateAdapterConstructor(Dictionary<MethodReference, MethodReference> methodMap);
    }
}