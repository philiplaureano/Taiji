using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Taiji
{
    public interface IMethodContext
    {
        CilWorker CilWorker { get; }
        VariableDefinition CurrentArguments { get; }
        MethodReference CurrentMethod { get; }
        VariableDefinition CurrentArgument { get; }
    }
}