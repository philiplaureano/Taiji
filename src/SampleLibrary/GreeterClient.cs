using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleLibrary
{
    public class GreeterClient
    {
        public void DoSomething()
        {
            var greeter = new Greeter(new Writer());
            greeter.Greet();
        }

        public void DoGreet(Writer writer)
        {
            writer.WriteLine("Hello, {0}", "World!");
        }
    }
}
