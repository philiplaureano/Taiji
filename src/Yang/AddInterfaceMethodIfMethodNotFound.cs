using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.Finders;
using LinFu.Finders.Interfaces;
using LinFu.IoC;
using LinFu.IoC.Configuration;
using LinFu.IoC.Interfaces;
using Mono.Cecil;
using LinFu.Reflection;

namespace Taiji.Yang
{
    [Implements(typeof(IMethodBuilder), ServiceName = "AddInterfaceMethodIfMethodNotFound")]
    public class AddInterfaceMethodIfMethodNotFound : IMethodBuilder, IInitialize<IServiceRequest>
    {
        private readonly IMethodFinder _finder;
        private readonly IMethodBuilder _newMethodBuilder;
        private IDictionary<MethodReference, MethodReference> _methodMap;

        public AddInterfaceMethodIfMethodNotFound(IMethodFinder finder, IMethodBuilder newMethodBuilder)
        {
            _finder = finder;
            _newMethodBuilder = newMethodBuilder;
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

        public void Initialize(IServiceRequest source)
        {
            var container = source.Container;
            _methodMap = container.GetService<IDictionary<MethodReference, MethodReference>>();
        }
    }
}
