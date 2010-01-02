using System.Collections.Generic;
using Mono.Cecil;

namespace TaijiConsole
{
    public class BaseDependencyScope : IDependencyScope
    {
        protected BaseDependencyScope()
        {
        }

        public virtual IEnumerable<MethodDefinition> Methods
        {
            get;
            protected set;
        }

        public virtual IEnumerable<MethodDefinition> Constructors
        {
            get;
            protected set;
        }

        public virtual IEnumerable<FieldDefinition> Fields
        {
            get;
            protected set;
        }
    }
}