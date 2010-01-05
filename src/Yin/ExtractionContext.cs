using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Taiji;

namespace Taiji.Yin
{
    public class ExtractionContext : IExtractionContext
    {
        private TypeReference _interfaceType;
        private TypeReference _targetDependency;
        private Dictionary<MethodReference, MethodReference> _methodMap;

        public ExtractionContext(TypeDefinition interfaceType, TypeReference targetDependency, Dictionary<MethodReference, MethodReference> methodMap)
        {
            _interfaceType = interfaceType;
            _targetDependency = targetDependency;
            _methodMap = methodMap;
        }

        public TypeReference InterfaceType
        {
            get { return _interfaceType; }
        }

        public TypeReference TargetDependency
        {
            get { return _targetDependency; }
        }

        public Dictionary<MethodReference, MethodReference> MethodMap
        {
            get { return _methodMap; }
        }
    }
}
