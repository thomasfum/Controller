using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Processor
{
    public class ControlUnit
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
            ALUInstruction alui = new ALUInstruction();
            alui.I= I;
            alui.A = A;
            alui.D = D;
            alui.CA = CA;
            val+=alui.proceed();

            ControlSelector cs= new ControlSelector();
            cs.s = I[15];
            cs.R1 = alui.R;
            cs.a1 = alui.a;
            cs.d1 = alui.d;
            cs.ca1 = alui.ca;
            cs.j1 = alui.j;


            cs.R0 = I;
            cs.a0 = true;
            cs.d0 = false;
            cs.ca0 = false;
            cs.j0 = false;
            val += cs.proceed();

            R= cs.R;
            a= cs.a;
            d= cs.d;
            ca= cs.ca;
            j= cs.j;

            return val;

        }
    }
}
