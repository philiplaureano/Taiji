using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace TaijiConsole
{
    public class AdapterBuilder : IAdapterBuilder
    {
        private readonly TypeDefinition _interfaceType;
        private readonly TypeReference _targetDependency;
        private readonly ITypeBuilder _createAdapterType;

        public AdapterBuilder(TypeDefinition interfaceType,
            TypeReference targetDependency, ITypeBuilder createAdapterType)
        {
            _interfaceType = interfaceType;
            _targetDependency = targetDependency;
            _createAdapterType = createAdapterType;
        }

        public MethodDefinition CreateAdapterConstructor(Dictionary<MethodReference, MethodReference> methodMap)
        {
            var adapterName = string.Format("{0}Adapter", _targetDependency.Name);
            var namespaceName = _interfaceType.Namespace;
            TypeDefinition adapterType = _createAdapterType.CreateType(adapterName, namespaceName, methodMap);
            var adapterConstructor = adapterType.Constructors[0];

            return adapterConstructor;
        }        
    }
}
