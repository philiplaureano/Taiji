using Mono.Cecil;

namespace Taiji
{
    public interface IParameterContext : IMethodContext
    {
        ParameterReference Parameter { get; }
        TypeReference InterfaceType { get; }
        TypeReference TargetDependency { get; }
        MethodDefinition AdapterConstructor { get; }
    }
}