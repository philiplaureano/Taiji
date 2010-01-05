using System.Collections.Generic;
using Mono.Cecil;

namespace Taiji.Yin
{
    public class DependencyScope : BaseDependencyScope
    {
        public DependencyScope(IEnumerable<MethodDefinition> methods, IEnumerable<MethodDefinition> constructors, IEnumerable<FieldDefinition> fields)
        {
            Methods = methods;
            Constructors = constructors;
            Fields = fields;
        }        
    }
}