using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleLibrary
{
    public class Greeter
    {
        private readonly Writer _writer;
        public Greeter(Writer writer)
        {
            _writer = writer;
        }
        public void Greet(Writer writer)
        {
            writer.WriteLine("Hello, {0}", "World!");
        }

        public virtual void Greet()
        {
            _writer.WriteLine("Hello, {0}", "World!");
        }

        protected virtual Writer Writer
        {
            get { return _writer; }
        }
    }
}
