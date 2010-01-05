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
    [Implements(typeof(IReplaceMethodCall), ServiceName = "PopMethodArguments")]
    public class PopMethodArguments : IReplaceMethodCall
    {
        public void Replace(IMethodContext context, ModuleDefinition targetModule)
        {
            var currentMethod = context.CurrentMethod;
            var currentArguments = context.CurrentArguments;
            var popMethod = targetModule.ImportMethod<Stack<object>>("Pop");
            var IL = context.CilWorker;
            foreach (ParameterReference param in currentMethod.Parameters)
            {
                IL.Emit(OpCodes.Ldloc, currentArguments);
                IL.Emit(OpCodes.Callvirt, popMethod);
                IL.Emit(OpCodes.Unbox_Any, param.ParameterType);
            }
        }
    }
}
