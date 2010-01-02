using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.Finders;
using LinFu.Finders.Interfaces;
using Mono.Cecil;

namespace TaijiConsole
{
    public class AddInterfaceMethodIfMethodNotFound : IMethodBuilder
    {
        private readonly IMethodFinder _finder;
        private readonly IMethodBuilder _newMethodBuilder;
        private readonly Dictionary<MethodReference, MethodReference> _methodMap;

        public AddInterfaceMethodIfMethodNotFound(IMethodFinder finder, IMethodBuilder newMethodBuilder, Dictionary<MethodReference, MethodReference> methodMap)
        {
            _finder = finder;
            _newMethodBuilder = newMethodBuilder;
            _methodMap = methodMap;
        }

        public MethodDefinition AddMethod(TypeDefinition targetType,
           MethodReference originalMethod)
        {
            if (originalMethod.Name == ".ctor")
                return null;

            MethodDefinition result = _finder.FindMethod(targetType, originalMethod); ;

            if (result != null)
                return result;

            result = _newMethodBuilder.AddMethod(targetType, originalMethod);

            // Map the interface method to the current method
            _methodMap[originalMethod] = result;

            return result;
        }        
    }
}
