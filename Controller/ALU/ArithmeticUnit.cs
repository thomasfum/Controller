using Controller.Arithmetics;
using Controller.Switching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.ALU
{
    public class ArithmeticUnit
    {
        public bool op0;
        public bool op1;
        public bool[] X = new bool[16];
        public bool[] Y = new bool[16];
        public bool[] output = new bool[16];

        public int proceed()
        {
            int val = 0;

            Selector_16 selector16_1 = new Selector_16();
            Selector_16 selector16_2 = new Selector_16();
            Adder_16 add = new Adder_16();
            Substracter_16 sub= new Substracter_16();


            selector16_1.s = op0;
            selector16_1.D0 = Y;
            selector16_1.D1[0] = true;
            val+=selector16_1.proceed();
            add.a = X;
            add.b = selector16_1.output;
            val += add.proceed();
            sub.a = X;
            sub.b = selector16_1.output;
            val += sub.proceed();

            selector16_2.s = op1;
            selector16_2.D0 = add.s;
            selector16_2.D1 = sub.s;
            val += selector16_2.proceed();
            output= selector16_2.output;
            return val;
        }
    }
}
