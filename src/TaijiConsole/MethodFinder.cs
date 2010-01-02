using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinFu.Finders;
using LinFu.Finders.Interfaces;
using Mono.Cecil;

namespace TaijiConsole
{
    public class MethodFinder : IMethodFinder
    {
        public MethodDefinition FindMethod(TypeDefinition interfaceType, MethodReference method)
        {
            var currentMethods = interfaceType.Methods.Cast<MethodDefinition>();
            var fuzzyList = currentMethods.AsFuzzyList();

            var position = 0;
            foreach (ParameterDefinition param in method.Parameters)
            {
                var parameterType = param.ParameterType;
                position++;
                Func<MethodDefinition, bool> matchesParameterType = m =>
                {
                    var parameters = m.Parameters;
                    var parameterCount = parameters.Count;

                    if (position >= parameterCount)
                        return false;

                    var targetParameter = parameters[position];

                    return targetParameter.ParameterType ==
                           parameterType;
                };

                fuzzyList.AddCriteria(matchesParameterType, CriteriaType.Optional);
            }

            // Match the method name
            Func<MethodDefinition, bool> matchesMethodName = m => m.Name == method.Name;

            fuzzyList.AddCriteria(matchesMethodName, CriteriaType.Critical);
            Func<MethodDefinition, bool>
                matchesReturnType = m =>
                {
                    var currentReturnType = m.ReturnType.ReturnType;
                    return currentReturnType == method.ReturnType.ReturnType;
                };

            fuzzyList.AddCriteria(matchesReturnType);

            var bestMatch = fuzzyList.BestMatch();
            if (bestMatch == null || bestMatch.Item == null)
                return null;

            return bestMatch.Item;
        }
    }
}
