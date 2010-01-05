using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Taiji
{
    public interface IReplaceMethodBody
    {
        void Replace(MethodBody currentBody, ICollection<MethodReference> modifiedItems);
    }
}