using System.Collections.Generic;
using Mono.Cecil;

namespace TaijiConsole
{
    public interface IDependencyScope
    {
        IEnumerable<MethodDefinition> Methods { get; }
        IEnumerable<MethodDefinition> Constructors { get; }
        IEnumerable<FieldDefinition> Fields { get; }
    }
}