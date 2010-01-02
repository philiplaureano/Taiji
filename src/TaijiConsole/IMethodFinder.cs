using Mono.Cecil;

namespace TaijiConsole
{
    public interface IMethodFinder
    {
        MethodDefinition FindMethod(TypeDefinition interfaceType, MethodReference method);
    }
}