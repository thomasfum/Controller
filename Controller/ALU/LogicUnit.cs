using Controller.Arithmetics;
using Controller.LogicGates;
using Controller.Switching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.ALU
{
    public class LogicUnit
    {
        public bool op0;
        public bool op1;
        public bool[] X = new bool[16];
        public bool[] Y = new bool[16];
        public bool[] output = new bool[16];
        public int proceed()
        {
            int val = 0;
            Inverter_16 inv16 = new Inverter_16();
            Xor_16 xor16 = new Xor_16();
            And_16  and16 = new And_16();
            Or_16 or16 = new Or_16();

            Selector_16 selector16_1 = new Selector_16();
            Selector_16 selector16_2 = new Selector_16();
            Selector_16 selector16_3 = new Selector_16();

            inv16.a = X;
            val += inv16.proceed();
            xor16.a = X;
            xor16.b = Y;
            val += xor16.proceed();
            or16.a = X;
            or16.b = Y;
            val += or16.proceed();
            and16.a = X;
            and16.b = Y;
            val += and16.proceed();

            selector16_1.s = op0;
            selector16_1.a = xor16.s;
            selector16_1.b= inv16.s;
            val += selector16_1.proceed();


            selector16_2.s = op0;
            selector16_2.a = and16.s;
            selector16_2.b = or16.s;
            val += selector16_2.proceed();


            selector16_3.s = op1;
            selector16_3.a = selector16_2.output;
            selector16_3.b = selector16_1.output;
            val += selector16_3.proceed();

            output= selector16_3.output;
            return val;
        }
    }
}
