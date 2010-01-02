using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace TaijiConsole
{
    public class DependencyExtractor
    {
        private readonly Dictionary<MethodReference, MethodReference> _methodMap;
        private readonly TypeReference _interfaceType;
        private readonly List<IDependencySwapper> _swappers = new List<IDependencySwapper>();
        public DependencyExtractor(TypeReference interfaceType, 
            Dictionary<MethodReference, MethodReference> methodMap, 
            HashSet<MethodReference> modifiedMethods, 
            HashSet<MethodReference> modifiedConstructors)
        {
            _methodMap = methodMap;
            _interfaceType = interfaceType;

            _swappers.Add(new SwapEmbeddedMethodTypeReferences(_interfaceType, modifiedMethods, modifiedConstructors));
            _swappers.Add(new SwapMethodCalls(_methodMap));
            _swappers.Add(new SwapMethodReturnTypes(_interfaceType));
            _swappers.Add(new SwapLocalVariableReferences(_interfaceType));
            _swappers.Add(new SwapFieldReferences(_interfaceType));
        }

        public void Extract(TypeDefinition targetDependency,
            ModuleDefinition hostModule,
            IDependencyScope scope)
        {            
            // Step 7: Replace all type instances of the target dependency with the new interface
            _swappers.ForEach(swapper => swapper.SwapDependencies(scope, targetDependency));
        }
    }
}
