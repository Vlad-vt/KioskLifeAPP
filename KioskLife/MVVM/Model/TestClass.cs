using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KioskLife.MVVM.Model
{
    internal class TestClass
    {
        public string Name { get; set; }

        public string Errors{ get; set; }

        public TestClass(string name, string errors)
        {
            Name = name;
            Errors = errors;
        }
    }
}
