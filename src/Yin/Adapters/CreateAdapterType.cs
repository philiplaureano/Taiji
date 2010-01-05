using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Taiji.Yin
{
    public class CreateAdapterType : ITypeBuilder
    {
        private readonly ModuleDefinition _targetModule;
        private readonly TypeReference _targetDependency;
        private readonly TypeDefinition _interfaceType;
        private readonly IAddAdapterConstructor _addAdapterConstructor;
        private readonly IAddAdapterMethod _addAdapterMethod;

        public CreateAdapterType(ModuleDefinition targetModule, 
            TypeReference targetDependency,
            TypeDefinition interfaceType, 
            IAddAdapterConstructor addAdapterConstructor, 
            IAddAdapterMethod addAdapterMethod)
        {
            _targetModule = targetModule;
            _targetDependency = targetDependency;
            _interfaceType = interfaceType;
            _addAdapterConstructor = addAdapterConstructor;
            _addAdapterMethod = addAdapterMethod;
        }

        public TypeDefinition CreateType(string adapterName, string namespaceName, Dictionary<MethodReference, MethodReference> methodMap)
        {
            const TypeAttributes adapterAttributes = TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.Class |
                                                     TypeAttributes.BeforeFieldInit;


            var adapterType = _targetModule.DefineClass(adapterName, namespaceName, adapterAttributes,
                                                 _targetModule.ImportType<object>());

            var fieldName = string.Format("___<>{0}__BackingField", _targetDependency.Name);
            var targetField = new FieldDefinition(fieldName, _targetDependency, FieldAttributes.Private);
            adapterType.Fields.Add(targetField);
            adapterType.Interfaces.Add(_interfaceType);

            _addAdapterConstructor.AddConstructor(adapterType, targetField);

            foreach (var method in _interfaceType.Methods.Cast<MethodDefinition>())
            {
                _addAdapterMethod.AddMethod(adapterType, targetField, method, methodMap);
            }

            return adapterType;
        }         
    }
}
