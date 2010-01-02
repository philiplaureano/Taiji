using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace TaijiConsole
{
    public class SwapMethodCalls : IDependencySwapper
    {
        private readonly Dictionary<MethodReference, MethodReference> _methodMap;

        public SwapMethodCalls(Dictionary<MethodReference, MethodReference> methodMap)
        {
            _methodMap = methodMap;
        }

        public void SwapDependencies(IDependencyScope scope, TypeDefinition targetDependency)
        {
            var targetMethods = scope.Methods;
            foreach (var method in targetMethods)
            {
                if (method.IsAbstract || !method.HasBody)
                    continue;

                var body = method.Body;
                body.InitLocals = true;
                var oldInstructions = body.Instructions.Cast<Instruction>().ToList();
                body.Instructions.Clear();

                SwapMethods(body, oldInstructions, _methodMap, targetDependency);
            }
        }        

        private static void SwapMethods(MethodBody body, IEnumerable<Instruction> oldInstructions, IDictionary<MethodReference, MethodReference> methodMap, 
            TypeDefinition targetDependency)
        {
            var IL = body.CilWorker;
            foreach (var instruction in oldInstructions)
            {
                var opCode = instruction.OpCode;
                if (opCode != OpCodes.Call && opCode != OpCodes.Callvirt)
                {
                    IL.Append(instruction);
                    continue;
                }

                var currentMethod = instruction.Operand as MethodReference;
                if (currentMethod == null || !methodMap.ContainsKey(currentMethod))
                {
                    IL.Append(instruction);
                    continue;
                }

                if (currentMethod.DeclaringType != targetDependency)
                {
                    IL.Append(instruction);
                    continue;
                }

                var interfaceMethod = methodMap[currentMethod];
                instruction.Operand = interfaceMethod;
                instruction.OpCode = OpCodes.Callvirt;

                IL.Append(instruction);
            }
        }

    }
}
