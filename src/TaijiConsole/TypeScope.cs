using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace TaijiConsole
{
    public class TypeScope : DependencyScope
    {
        public TypeScope(TypeDefinition targetType)
            : base(targetType.Methods.Cast<MethodDefinition>(),
            targetType.Constructors.Cast<MethodDefinition>(),
            targetType.Fields.Cast<FieldDefinition>())
        {
        }       
    }
}
