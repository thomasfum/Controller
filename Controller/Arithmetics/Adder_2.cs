using Controller;
using Controller.Arithmetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    // adder 2 bits
    public class Adder_2
    {
        public bool a0;
        public bool a1;
        public bool b0;
        public bool b1;
        public bool c;
        

        public bool s0;
        public bool s1;
        public bool cOut;
        FullAdder add1 = new FullAdder();
        FullAdder add2 = new FullAdder();
        public int proceed()
        {
            int val = 0;
            add1.a = a0;
            add1.b = b0;
            add1.c = c;
            val+=add1.proceed();
            s0 = add1.l;
            add2.a = a1;
            add2.b = b1;
            add2.c = add1.h;
            val += add2.proceed();
            s1 = add2.l;
            cOut= add2.h;
            return val;
        }

    }
}
