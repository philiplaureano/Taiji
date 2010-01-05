using Mono.Cecil;

namespace Taiji
{
    public interface IReplaceMethodCall
    {
        void Replace(IMethodContext methodContext, ModuleDefinition module);
    }
}