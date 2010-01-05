using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taiji
{
    public interface IAdaptParameter
    {
        void Adapt(IParameterContext context);
    }
}
