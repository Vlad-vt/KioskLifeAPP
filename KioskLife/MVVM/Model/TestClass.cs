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

        public int Price{ get; set; }

        public TestClass(string name, int price)
        {
            Name = name;
            Price = price;
        }
    }
}
