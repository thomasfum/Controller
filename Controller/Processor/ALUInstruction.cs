using Controller.ALU;
using Controller.Switching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Processor
{
    public class ALUInstruction
    {

        public bool[] I = new bool[16];
        public bool[] A = new bool[16];
        public bool[] D = new bool[16];
        public bool[] CA = new bool[16];

        public bool[] R = new bool[16];//output

        public bool a;
        public bool d;
        public bool ca;
        public bool j;

        public int proceed()
        {
            int val = 0;
            Selector_16 select = new Selector_16();
            ALU.ALU alu = new ALU.ALU();
            Condition condition = new Condition();

            condition.gt =  I[0];
            condition.eq =  I[1];
            condition.lt =  I[2];
            ca =            I[3];
            d =             I[4];   
            a =             I[5];
            alu.sw =        I[6];
            alu.zx =        I[7];
            alu.op0 =       I[8];
            alu.op1 =       I[9];
            alu.u =         I[10];

            select.s        = I[12];

            select.D0 = A;
            select.D1 = CA;

            val+=select.proceed();

            alu.X = D;
            alu.Y = select.output;
            val += alu.proceed();

            condition.X = alu.output;
            val += condition.proceed();
            j = condition.output;
            R = alu.output;

            return val;
        }

    }
}
