using Mono.Cecil;

namespace Taiji
{
    public interface IMethodFinder
    {
        MethodDefinition FindMethod(TypeDefinition interfaceType, MethodReference method);
    }
}