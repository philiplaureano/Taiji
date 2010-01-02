using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace TaijiConsole
{
    public class ParameterContext : MethodContext
    {
        public ParameterContext(CilWorker worker, TypeReference interfaceType, MethodReference currentMethod, VariableDefinition currentArguments, VariableDefinition currentArgument, TypeReference targetDependency, MethodDefinition adapterConstructor, ParameterReference param)
        {
            CilWorker = worker;
            CurrentArguments = currentArguments;            
            CurrentArgument = currentArgument;            
            TargetDependency = targetDependency;

            Parameter = param;
            InterfaceType = interfaceType;
            AdapterConstructor = adapterConstructor;
            CurrentMethod = currentMethod;
        }

        public ParameterReference Parameter
        {
            get; private set;
        }

        public TypeReference InterfaceType
        {
            get; private set;
        }

        public TypeReference TargetDependency
        {
            get; private set;
        }

        public MethodDefinition AdapterConstructor
        {
            get; private set;
        }        
    }
}
