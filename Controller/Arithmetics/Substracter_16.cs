using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Arithmetics
{
    public class Substracter_16
    {

        public bool[] a = new bool[16];
        public bool[] b = new bool[16];
        public bool[] s = new bool[16];

        public int proceed()
        {
            int val = 0;
            Adder_16 add= new Adder_16();
            Inverter_16 inv = new Inverter_16();
            inv.a = b;
            val+=inv.proceed();
            add.a= a;
            add.b = inv.s;
            add.c = true;
            val += add.proceed();
            s = add.s;
            return val;
        }
    }
}
