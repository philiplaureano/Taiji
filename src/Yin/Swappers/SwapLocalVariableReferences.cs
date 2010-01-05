using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Taiji.Yin
{
    public class SwapLocalVariableReferences : IDependencySwapper
    {
        private readonly TypeReference _interfaceType;

        public SwapLocalVariableReferences(TypeReference interfaceType)
        {
            _interfaceType = interfaceType;
        }

        public void SwapDependencies(IDependencyScope scope, TypeDefinition targetDependency)
        {
            var methods = scope.Methods;
            foreach (var method in methods.Cast<MethodDefinition>())
            {
                if (method.IsAbstract || !method.HasBody)
                    continue;

                var body = method.Body;
                var locals = body.Variables.Cast<VariableDefinition>();

                foreach (var local in locals)
                {
                    if (local.VariableType != targetDependency)
                        continue;

                    local.VariableType = _interfaceType;
                }
            }
        }
    }
}
