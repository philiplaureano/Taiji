using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace TaijiConsole
{
    public interface IInvalidCallFilter
    {
        Dictionary<object, Instruction> GetInvalidCalls(MethodBody currentBody,
                                                                        ICollection<MethodReference> modifiedItems);
    }
}