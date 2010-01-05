using Mono.Cecil;

namespace Taiji
{
    public interface IAddAdapterConstructor
    {
        void AddConstructor(TypeDefinition adapterType,
                                            FieldReference targetField);
    }
}