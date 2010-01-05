using System.Collections.Generic;
using Mono.Cecil;

namespace Taiji
{
    public interface IDependencyScope
    {
        IEnumerable<MethodDefinition> Methods { get; }
        IEnumerable<MethodDefinition> Constructors { get; }
        IEnumerable<FieldDefinition> Fields { get; }
    }
}