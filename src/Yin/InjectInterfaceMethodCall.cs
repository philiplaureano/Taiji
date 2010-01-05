using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Taiji.Yin
{
    public class InjectInterfaceMethodCall : IReplaceMethodCall
    {
        private readonly IReplaceMethodCall _pushMethodArguments;
        private readonly IReplaceMethodCall _popMethodArguments;

        public InjectInterfaceMethodCall(IReplaceMethodCall pushMethodArguments,
            IReplaceMethodCall popMethodArguments)
        {
            _pushMethodArguments = pushMethodArguments;
            _popMethodArguments = popMethodArguments;
        }

        public void Replace(IMethodContext methodContext, ModuleDefinition module)
        {
            // Put all the method arguments into the argument stack
            _pushMethodArguments.Replace(methodContext, module);

            // Push the arguments back onto the stack
            _popMethodArguments.Replace(methodContext, module);
        }                  
    }
}
