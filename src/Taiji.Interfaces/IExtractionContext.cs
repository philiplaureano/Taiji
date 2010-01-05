using System.Collections.Generic;
using Mono.Cecil;

namespace Taiji
{
    public interface IExtractionContext
    {
        TypeReference InterfaceType { get; }
        TypeReference TargetDependency { get; }
        Dictionary<MethodReference, MethodReference> MethodMap { get; }
    }
}