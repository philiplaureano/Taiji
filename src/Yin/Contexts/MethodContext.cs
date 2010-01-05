using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Taiji.Yin
{
    public class MethodContext : IMethodContext
    {
        protected MethodContext()
        {            
        }

        public MethodContext(CilWorker worker, VariableDefinition currentArguments, MethodReference currentMethod, VariableDefinition currentArgument)
        {
            CilWorker = worker;
            CurrentArguments = currentArguments;
            CurrentMethod = currentMethod;
            CurrentArgument = currentArgument;
        }

        public CilWorker CilWorker
        {
            get;
            protected set;
        }

        public VariableDefinition CurrentArguments
        {
            get;
            protected set;
        }

        public MethodReference CurrentMethod
        {
            get;
            protected set;
        }

        public VariableDefinition CurrentArgument
        {
            get;
            protected set;
        }
    }
}
