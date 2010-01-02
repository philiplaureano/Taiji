using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleLibrary
{
    public sealed class Writer
    {
        public void Write(string message)
        {
            Console.Write(message);
        }

        public void WriteLine(string format, params object[] formatArgs)
        {
            Console.WriteLine(format, formatArgs);
        }
    }
}
