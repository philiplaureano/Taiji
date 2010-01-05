using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace Taiji.Yin
{
    public class ModuleScope : BaseDependencyScope
    {
        private readonly ModuleDefinition _module;

        public ModuleScope(ModuleDefinition module)
        {
            _module = module;
        }

        public override IEnumerable<MethodDefinition> Constructors
        {
            get
            {
                return GetResults(t => t.Constructors.Cast<MethodDefinition>());
            }
            protected set
            {
                throw new NotSupportedException();
            }
        }        

        public override IEnumerable<FieldDefinition> Fields
        {
            get
            {
                return GetResults(t => t.Fields.Cast<FieldDefinition>());
            }

            protected set
            {
                throw new NotSupportedException();
            }
        }

        public override IEnumerable<MethodDefinition> Methods
        {
            get
            {
                return GetResults(t => t.Methods.Cast<MethodDefinition>());
            }
            protected set
            {
                throw new NotSupportedException();
            }
        }

        private IEnumerable<T> GetResults<T>(Func<TypeDefinition, IEnumerable<T>> getItems)
        {
            var results = new HashSet<T>();
            var types = _module.Types.Cast<TypeDefinition>();

            AddResults(types, t => getItems(t).Cast<T>(), results);
            return results;
        }

        private void AddResults<T>(IEnumerable<TypeDefinition> types, Func<TypeDefinition, IEnumerable<T>> getItems, HashSet<T> results)
        {
            foreach (var type in types)
            {
                if (type.Name == "<Module>")
                    continue;

                AddItems(type, getItems, results);
            }
        }

        private void AddItems<T>(TypeDefinition type, Func<TypeDefinition, IEnumerable<T>> getItems, HashSet<T> results)
        {
            var items = getItems(type);
            foreach (var item in items.Cast<T>())
            {
                results.Add(item);
            }
        }
    }
}
