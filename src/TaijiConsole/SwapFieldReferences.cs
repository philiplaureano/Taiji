using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace TaijiConsole
{
    public class SwapFieldReferences : IDependencySwapper
    {
        private readonly TypeReference _interfaceType;
        public SwapFieldReferences(TypeReference interfaceType)
        {
            _interfaceType = interfaceType;
        }

        public void SwapDependencies(IDependencyScope scope, TypeDefinition targetDependency)
        {
            var fields = scope.Fields;
            foreach (var field in fields)
            {
                var fieldType = field.FieldType;
                if (fieldType != targetDependency)
                    continue;

                field.FieldType = _interfaceType;
            }
        }
    }
}
