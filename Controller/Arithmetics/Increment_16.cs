using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Arithmetics
{
    //increment 16 bits
    public class Increment_16
    {
        public bool[] a = new bool[16];
        public bool[] s = new bool[16];
        public int proceed()
        {
            Adder_16 adder = new Adder_16();
            adder.a = a;
            adder.b = new bool[16];//assuming false
            adder.c = true;
            int val=adder.proceed();
            s=adder.s;
            return val;
        }
    }
}
