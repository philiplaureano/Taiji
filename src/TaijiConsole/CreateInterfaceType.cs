using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.Finders;
using LinFu.Finders.Interfaces;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace TaijiConsole
{
    public class CreateInterfaceType : ITypeBuilder 
    {
        private readonly IMethodFilter _methodFilter;
        private readonly IMethodBuilder _methodBuilder;
        private readonly IDependencyScope _scope;
        private readonly TypeDefinition _targetDependency;
        private readonly ModuleDefinition _module;

        public CreateInterfaceType(IMethodFilter methodFilter, 
            IMethodBuilder methodBuilder, 
            IDependencyScope scope, 
            TypeDefinition targetDependency,
            ModuleDefinition module)
        {
            _methodFilter = methodFilter;
            _methodBuilder = methodBuilder;
            _scope = scope;
            _targetDependency = targetDependency;
            _module = module;
        }

        public TypeDefinition CreateType(string interfaceName, 
            string targetNamespace,
           Dictionary<MethodReference, MethodReference> methodMap)
        {
            var targetMethods = _methodFilter.GetMethods(_scope.Methods, _targetDependency);

            // Step 6
            const TypeAttributes attributes = TypeAttributes.Interface |
                             TypeAttributes.Abstract | TypeAttributes.Public;

            var interfaceType = new TypeDefinition(interfaceName, targetNamespace, attributes, null);
            interfaceType.Constructors.Clear();

            _module.Types.Add(interfaceType);

            foreach (var method in targetMethods)
            {
                _methodBuilder.AddMethod(interfaceType, method);
            }

            return interfaceType;
        }       
    }
}
