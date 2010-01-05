using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.IoC.Configuration;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Taiji.Yang
{
    [Implements(typeof(IMethodFilter))]
    [Implements(typeof(IMethodFilter), ServiceName = "InvokedMethodFilter")]
    public class InvokedMethodFilter : IMethodFilter
    {
        public IEnumerable<MethodReference> GetMethods(IEnumerable<MethodDefinition> targetMethods,
TypeDefinition targetDependency)
        {
            var results = new HashSet<MethodReference>();
            foreach (var method in targetMethods)
            {
                var body = method.Body;
                var instructions = body.Instructions.Cast<Instruction>();

                // Step 5
                var methods = (from instruction in instructions
                               let currentMethod = instruction.Operand as MethodReference
                               let opcode = instruction.OpCode
                               where opcode == OpCodes.Call || opcode == OpCodes.Callvirt && currentMethod != null
                                                               && currentMethod.DeclaringType == targetDependency &&
                                                               currentMethod.HasThis
                               select currentMethod).ToList();

                Action<MethodReference> addItem = m =>
                {
                    if (results.Contains(m))
                        return;

                    results.Add(m);
                };

                methods.ForEach(addItem);
            }

            return results.ToList();
        }
    }
}
