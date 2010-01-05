using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Taiji
{
    public interface IInvalidCallFilter
    {
        Dictionary<object, Instruction> GetInvalidCalls(MethodBody currentBody,
                                                                        ICollection<MethodReference> modifiedItems);
    }
}