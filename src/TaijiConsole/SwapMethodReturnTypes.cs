using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace TaijiConsole
{
    public class SwapMethodReturnTypes : IDependencySwapper
    {
        private readonly TypeReference _interfaceType;
        public SwapMethodReturnTypes(TypeReference interfaceType)
        {
            _interfaceType = interfaceType;
        }

        public void SwapDependencies(IDependencyScope scope, TypeDefinition targetDependency)
        {
            foreach(var method in scope.Methods)
            {
                var returnType = method.ReturnType.ReturnType;
                if (returnType != targetDependency)
                    continue;

                method.ReturnType = new MethodReturnType(_interfaceType);
            }
        }
    }
}
