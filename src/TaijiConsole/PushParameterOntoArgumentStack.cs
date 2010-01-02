using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace TaijiConsole
{
    public class PushParameterOntoArgumentStack : IAdaptParameter
    {
        private readonly IAdaptParameter _adaptParameter;        
        public PushParameterOntoArgumentStack(IAdaptParameter adaptParameter)
        {
            _adaptParameter = adaptParameter;
        }

        public void Adapt(ParameterContext context)
        {
            var pushParameterArguments = context;
            var parameterType = pushParameterArguments.Parameter.ParameterType;
            if (parameterType.IsValueType || parameterType is GenericParameter)
                pushParameterArguments.CilWorker.Emit(OpCodes.Box);

            var worker = pushParameterArguments.CilWorker;
            worker.Emit(OpCodes.Stloc, pushParameterArguments.CurrentArgument);

            // If necessary, wrap the old dependency in the adapter
            if (parameterType == pushParameterArguments.InterfaceType)
                _adaptParameter.Adapt(pushParameterArguments);

            var currentArguments = pushParameterArguments.CurrentArguments;
            var currentArgument = pushParameterArguments.CurrentArgument;

            var pushMethod = pushParameterArguments.CurrentMethod;
            worker.Emit(OpCodes.Ldloc, currentArguments);
            worker.Emit(OpCodes.Ldloc, currentArgument);
            worker.Emit(OpCodes.Callvirt, pushMethod);
        }
    }
}
