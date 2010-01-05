using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.IoC.Configuration;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Taiji.Yin
{
    [Implements(typeof(IAddAdapterMethod))]
    public class AddAdapterMethod : IAddAdapterMethod
    {
        public void AddMethod(TypeDefinition adapterType,
           FieldReference targetField,
           MethodDefinition targetMethod,
           Dictionary<MethodReference, MethodReference> methodMap)
        {
            const MethodAttributes methodAttributes =
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot |
                MethodAttributes.Virtual;

            var methodName = targetMethod.Name;
            var returnType = targetMethod.ReturnType.ReturnType;

            // Match the method parameters
            var newMethod = new MethodDefinition(methodName, methodAttributes, returnType);
            AddParameters(targetMethod, newMethod);

            var body = newMethod.Body;
            body.InitLocals = true;
            var IL = body.CilWorker;

            IL.Emit(OpCodes.Ldarg_0);
            IL.Emit(OpCodes.Ldfld, targetField);

            var index = 1;
            foreach (var param in targetMethod.Parameters.Cast<ParameterDefinition>())
            {
                IL.Emit(OpCodes.Ldarg, index++);
            }

            var currentMethod = targetMethod;
            var originalMethod = (from entry in methodMap
                                  where entry.Value == currentMethod
                                  select entry.Key).First();

            newMethod.Overrides.Add(targetMethod);
            var callOpCode = targetMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call;
            IL.Emit(callOpCode, originalMethod);
            IL.Emit(OpCodes.Ret);

            adapterType.Methods.Add(newMethod);
        }

        private void AddParameters(IMethodSignature method, IMethodSignature newMethod)
        {
            foreach (var param in method.Parameters.Cast<ParameterDefinition>())
            {
                var newParameter = new ParameterDefinition(param.ParameterType);
                newMethod.Parameters.Add(newParameter);
            }
        }     
    }
}
