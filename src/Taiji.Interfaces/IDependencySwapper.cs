using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Taiji
{
    public interface IDependencySwapper
    {
        void SwapDependencies(IDependencyScope scope, TypeDefinition targetDependency);
    }
}
