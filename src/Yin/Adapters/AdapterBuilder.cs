using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.IoC;
using LinFu.IoC.Configuration;
using LinFu.IoC.Interfaces;
using LinFu.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Taiji.Yin
{
    [Implements(typeof(IAdapterBuilder))]
    public class AdapterBuilder : IAdapterBuilder, IInitialize
    {
        private readonly TypeReference _interfaceType;
        private readonly TypeReference _targetDependency;
        private ITypeBuilder _createAdapterType;

        public AdapterBuilder(IExtractionContext context)
        {
            _interfaceType = context.InterfaceType;
            _targetDependency = context.TargetDependency;
        }

        public MethodDefinition CreateAdapterConstructor(Dictionary<MethodReference, MethodReference> methodMap)
        {
            var adapterName = string.Format("{0}Adapter", _targetDependency.Name);
            var namespaceName = _interfaceType.Namespace;
            TypeDefinition adapterType = _createAdapterType.CreateType(adapterName, namespaceName, methodMap);
            var adapterConstructor = adapterType.Constructors[0];

            return adapterConstructor;
        }

        public void Initialize(IServiceContainer source)
        {
            _createAdapterType = source.GetService<ITypeBuilder>("CreateAdapterType");
        }
    }
}
