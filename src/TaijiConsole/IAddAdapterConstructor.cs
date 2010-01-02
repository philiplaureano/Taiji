using Mono.Cecil;

namespace TaijiConsole
{
    public interface IAddAdapterConstructor
    {
        void AddConstructor(TypeDefinition adapterType,
                                            FieldReference targetField);
    }
}