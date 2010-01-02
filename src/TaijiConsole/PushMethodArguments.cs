using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace TaijiConsole
{
    public class PushMethodArguments : IReplaceMethodCall
    {
        private readonly TypeReference _targetDependency;
        private readonly TypeReference _interfaceType;
        private readonly IAdapterBuilder _adapterBuilder;
        private readonly IAdaptParameter _pushParameter;
        private readonly Dictionary<MethodReference, MethodReference> _methodMap;

        public PushMethodArguments(IAdapterBuilder adapterBuilder,
            IAdaptParameter pushParameter,
            TypeReference interfaceType,
            TypeReference targetDependency,
            Dictionary<MethodReference, MethodReference> methodMap)
        {
            _targetDependency = targetDependency;
            _interfaceType = interfaceType;
            _adapterBuilder = adapterBuilder;
            _pushParameter = pushParameter;
            _methodMap = methodMap;
        }

        public void Replace(IMethodContext context, ModuleDefinition targetModule)
        {
            var currentMethod = context.CurrentMethod;
            var currentArguments = context.CurrentArguments;
            var currentArgument = context.CurrentArgument;
            var pushMethod = targetModule.ImportMethod<Stack<object>>("Push");
            var worker = context.CilWorker;

            var adapterConstructor = _adapterBuilder.CreateAdapterConstructor(_methodMap);
            foreach (ParameterReference param in currentMethod.Parameters)
            {
                var arguments = new ParameterContext(worker,
                _interfaceType,
                pushMethod,
                currentArguments,
                currentArgument,
                _targetDependency, adapterConstructor, param);

                // Save the current argument
                _pushParameter.Adapt(arguments);
            }
        }        
    }
}
