using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Taiji.Yin
{
    public class AddAdapterConstructor : IAddAdapterConstructor
    {
        private readonly TypeReference _targetDependency;

        public AddAdapterConstructor(IExtractionContext context)
        {
            _targetDependency = context.TargetDependency;
        }

        public void AddConstructor(TypeDefinition adapterType,
           FieldReference targetField)
        {
            var adapterCtor = adapterType.AddDefaultConstructor();
            var adapterParameter = new ParameterDefinition(_targetDependency);
            adapterCtor.Parameters.Add(adapterParameter);

            // HACK: Remove the ret instruction from the default constructor and replace it with
            // the field setter 
            var adapterBody = adapterCtor.Body;
            var adapterInstructions = adapterBody.Instructions.Cast<Instruction>().Where(i => i.OpCode != OpCodes.Ret).ToArray();
            adapterBody.Instructions.Clear();

            // Copy the old instructions
            var IL = adapterBody.CilWorker;
            foreach (var instruction in adapterInstructions)
            {
                IL.Append(instruction);
            }

            IL.Emit(OpCodes.Ldarg_0);
            IL.Emit(OpCodes.Ldarg, adapterParameter);
            IL.Emit(OpCodes.Stfld, targetField);
            IL.Emit(OpCodes.Ret);
        }
    }
}
