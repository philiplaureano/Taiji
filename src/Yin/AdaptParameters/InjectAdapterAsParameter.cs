using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.IoC.Configuration;
using Mono.Cecil.Cil;

namespace Taiji.Yin
{
    [Implements(typeof(IAdaptParameter), ServiceName = "InjectAdapterAsParameter")]
    public class InjectAdapterAsParameter : IAdaptParameter
    {
        public void Adapt(IParameterContext context)
        {
            var IL = context.CilWorker;
            var adapterConstructor = context.AdapterConstructor;
            var targetDependency = context.TargetDependency;
            var currentArgument = context.CurrentArgument;
            var interfaceType = context.InterfaceType;

            IL.Emit(OpCodes.Ldloc, currentArgument);
            IL.Emit(OpCodes.Unbox_Any, targetDependency);
            IL.Emit(OpCodes.Newobj, adapterConstructor);
            IL.Emit(OpCodes.Isinst, interfaceType);
            IL.Emit(OpCodes.Stloc, currentArgument);
        }
    }
}
