using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace TaijiConsole
{
    public class InvalidCallFilter : IInvalidCallFilter
    {
        public Dictionary<object, Instruction> GetInvalidCalls(MethodBody currentBody,
           ICollection<MethodReference> modifiedItems)
        {
            var currentInstructions = currentBody.Instructions.Cast<Instruction>().ToArray();

            HashSet<OpCode> validOpCodes = GetValidMethodCallOpCodes();

            // Determine the call instructions that have been invalidated since
            // the interface extraction
            return (from instruction in currentInstructions
                    let method = instruction.Operand as MethodReference
                    where method != null && modifiedItems.Contains(method) &&
                          validOpCodes.Contains(instruction.OpCode)
                    select instruction).ToDictionary(i => i.Operand);
        }

        private HashSet<OpCode> GetValidMethodCallOpCodes()
        {
            var validOpCodes = new HashSet<OpCode>();
            validOpCodes.Add(OpCodes.Call);
            validOpCodes.Add(OpCodes.Callvirt);
            validOpCodes.Add(OpCodes.Newobj);
            return validOpCodes;
        }
    }
}
