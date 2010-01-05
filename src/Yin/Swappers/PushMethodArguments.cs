using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.IoC.Configuration;
using LinFu.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Taiji.Yin
{    
    [Implements(typeof(IReplaceMethodCall), ServiceName = "PushMethodArguments")]
    public class PushMethodArguments : IReplaceMethodCall
    {
        private readonly IAdapterBuilder _adapterBuilder;
        private readonly IAdaptParameter _pushParameter;
        private readonly IExtractionContext _context;

        public PushMethodArguments(IAdapterBuilder adapterBuilder,
            IAdaptParameter pushParameter, IExtractionContext extractionContext)
        {
            _adapterBuilder = adapterBuilder;
            _pushParameter = pushParameter;
            _context = extractionContext;
        }

        public void Replace(IMethodContext context, ModuleDefinition targetModule)
        {
            var currentMethod = context.CurrentMethod;
            var currentArguments = context.CurrentArguments;
            var currentArgument = context.CurrentArgument;
            var pushMethod = targetModule.ImportMethod<Stack<object>>("Push");
            var worker = context.CilWorker;

            var methodMap = _context.MethodMap;
            var targetDependency = _context.TargetDependency;
            var interfaceType = _context.InterfaceType;

            var adapterConstructor = _adapterBuilder.CreateAdapterConstructor(methodMap);
            foreach (ParameterReference param in currentMethod.Parameters)
            {
                var arguments = new ParameterContext(worker,
                interfaceType,
                pushMethod,
                currentArguments,
                currentArgument,
                targetDependency, adapterConstructor, param);

                // Save the current argument
                _pushParameter.Adapt(arguments);
            }
        }
    }
}
