using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.Reflection.Emit;
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace Taiji.Yin
{
    public class ReplaceMethodCalls : IReplaceMethodBody
    {
        private readonly ModuleDefinition _targetModule;
        private readonly IInvalidCallFilter _callFilter;
        private readonly IReplaceMethodCall _replaceMethodCall;
        
        public ReplaceMethodCalls(ModuleDefinition targetModule,
            IReplaceMethodCall replaceMethodCall, IInvalidCallFilter callFilter)
        {
            _targetModule = targetModule;
            _replaceMethodCall = replaceMethodCall;
            _callFilter = callFilter;
        }

        public void Replace(MethodBody currentBody, ICollection<MethodReference> modifiedItems)
        {
            var invalidCalls = _callFilter.GetInvalidCalls(currentBody, modifiedItems);
            if (invalidCalls.Count == 0)
                return;

            var currentInstructions = currentBody.Instructions.Cast<Instruction>().ToArray();
            var stackCtor = _targetModule.ImportConstructor<Stack<object>>(new Type[0]);
            var IL = currentBody.CilWorker;

            var targetMethod = currentBody.Method;
            var currentArgument = targetMethod.AddLocal<object>();
            var currentArguments = targetMethod.AddLocal<Stack<object>>();            
            
            currentBody.Instructions.Clear();

            // Create the stack that will hold the method arguments
            IL.Emit(OpCodes.Newobj, stackCtor);
            IL.Emit(OpCodes.Stloc, currentArguments);
            foreach (var currentInstruction in currentInstructions)
            {
                var currentMethod = currentInstruction.Operand as MethodReference;

                // Ignore any instructions that weren't affected by the 
                // interface extraction
                if (currentMethod != null && invalidCalls.ContainsKey(currentMethod))
                {
                    var context = new MethodContext(IL, currentArguments, currentMethod, currentArgument);
                    _replaceMethodCall.Replace(context, _targetModule);
                }

                IL.Append(currentInstruction);
            }
        }
    }
}
