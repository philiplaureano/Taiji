using Mono.Cecil;

namespace TaijiConsole
{
    public interface IReplaceMethodCall
    {
        void Replace(IMethodContext methodContext, ModuleDefinition module);
    }
}