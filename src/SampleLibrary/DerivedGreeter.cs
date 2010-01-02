using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleLibrary
{
    public class DerivedGreeter : Greeter
    {
        public DerivedGreeter(Writer writer) : base(writer)
        {            
        }

        public override void Greet()
        {
            Writer.Write("Hello, DerivedWorld!");
        }
    }
}
