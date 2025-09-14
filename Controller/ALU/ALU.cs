using Controller.Switching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.ALU
{
    public class ALU
    {

        public bool u;
        public bool op0;
        public bool op1;
        public bool zx;
        public bool sw;
        public bool[] X = new bool[16];
        public bool[] Y = new bool[16];
        public bool[] output = new bool[16];
        public int proceed()
        {
            int val = 0;
            Selector_16 selector16_Swap_1 = new Selector_16();
            Selector_16 selector16_Swap_2 = new Selector_16();
            Selector_16 selector16_zero = new Selector_16();
            Selector_16 selector16_AL = new Selector_16();

            ArithmeticUnit AU = new ArithmeticUnit();
            LogicUnit LU = new LogicUnit();

            selector16_Swap_1.s = sw;
            selector16_Swap_1.a = X;
            selector16_Swap_1.b = Y;
            val+=selector16_Swap_1.proceed();

            selector16_Swap_2.s = sw;
            selector16_Swap_2.a = Y;
            selector16_Swap_2.b = X;
            val += selector16_Swap_2.proceed();

            selector16_zero.s = zx;
            selector16_zero.a = selector16_Swap_1.output;
            val += selector16_zero.proceed();

            LU.X = selector16_zero.output;
            LU.Y= selector16_Swap_2.output;
            LU.op0 = op0;
            LU.op1 = op1;
            val += LU.proceed();


            AU.X = selector16_zero.output;
            AU.Y = selector16_Swap_2.output;
            AU.op0 = op0;
            AU.op1 = op1;
            val += AU.proceed();

            selector16_AL.s = u;
            selector16_AL.a= LU.output;
            selector16_AL.b = AU.output;
            val += selector16_AL.proceed();
            output = selector16_AL.output;

            return val;
        }
    }
}
