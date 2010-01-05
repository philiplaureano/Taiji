using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Taiji.Yin
{
    public class SwapEmbeddedMethodTypeReferences : IDependencySwapper
    {
        private readonly TypeReference _interfaceType;
        private readonly HashSet<MethodReference> _modifiedMethods;
        private readonly HashSet<MethodReference> _modifiedConstructors;

        public SwapEmbeddedMethodTypeReferences(TypeReference interfaceType, HashSet<MethodReference> modifiedMethods, HashSet<MethodReference> modifiedConstructors)
        {
            _interfaceType = interfaceType;
            _modifiedMethods = modifiedMethods;
            _modifiedConstructors = modifiedConstructors;
        }

        public void SwapDependencies(IDependencyScope scope, TypeDefinition targetDependency)
        {
            var targetMethods = scope.Methods;
            var targetConstructors = scope.Constructors;

            targetMethods.ForEach(method => SwapParameterTypes(method, targetDependency, _interfaceType, _modifiedMethods));
            targetConstructors.ForEach(method => SwapParameterTypes(method, targetDependency, _interfaceType, _modifiedConstructors));
        }

        private static void SwapParameterTypes(MethodDefinition method,
            TypeDefinition targetDependency,
            TypeReference interfaceType,
            HashSet<MethodReference> modifiedMethods)
        {
            if (method.IsAbstract || !method.HasBody)
                return;

            bool modified = false;
            var parameters = method.Parameters.Cast<ParameterDefinition>();
            foreach (var parameter in parameters)
            {
                var parameterType = parameter.ParameterType;
                if (parameterType != targetDependency)
                    continue;

                parameter.ParameterType = interfaceType;
                modified = true;
            }

            if (!modified)
                return;

            modifiedMethods.Add(method);
        }
    }
}
