using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.Reflection.Emit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace TaijiConsole
{
    public class DependencyUpdater
    {
        private readonly IReplaceMethodBody _replaceMethodBody;

        public DependencyUpdater(IReplaceMethodBody replaceMethodBody)
        {
            _replaceMethodBody = replaceMethodBody;
        }

        public void UpdateAffectedMethods(IEnumerable<MethodDefinition> targetMethods,
            ICollection<MethodReference> modifiedMethods,
            Dictionary<MethodReference, MethodReference> methodMap)
        {
            foreach (var targetMethod in targetMethods)
            {
                UpdateMethod(targetMethod, modifiedMethods, methodMap);
            }
        }

        private void UpdateMethod(MethodDefinition targetMethod,
            ICollection<MethodReference> modifiedItems,
            Dictionary<MethodReference, MethodReference> methodMap)
        {
            if (targetMethod.IsAbstract || !targetMethod.HasBody)
                return;

            var currentBody = targetMethod.Body;            

            _replaceMethodBody.Replace(currentBody, modifiedItems);
        }       
    }
}
