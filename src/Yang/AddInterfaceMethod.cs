using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.IoC.Configuration;
using Mono.Cecil;

namespace Taiji.Yang
{
    [Implements(typeof(IMethodBuilder))]
    [Implements(typeof(IMethodBuilder), ServiceName = "AddInterfaceMethod")]
    public class AddInterfaceMethod : IMethodBuilder
    {
        public MethodDefinition AddMethod(TypeDefinition targetInterfaceType, MethodReference prototypeMethod)
        {
            var methodName = prototypeMethod.Name;
            var returnType = prototypeMethod.ReturnType.ReturnType;
            const MethodAttributes methodAttributes =
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot |
                MethodAttributes.Virtual | MethodAttributes.Abstract;

            var newMethod = new MethodDefinition(methodName, methodAttributes, returnType) { IsManaged = true };

            // Match the method signature
            foreach (ParameterDefinition param in prototypeMethod.Parameters)
            {
                var newParameter = new ParameterDefinition(param.ParameterType);
                newMethod.Parameters.Add(newParameter);
            }

            targetInterfaceType.Methods.Add(newMethod);
            return newMethod;
        }
    }
}
