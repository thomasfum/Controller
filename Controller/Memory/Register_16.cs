using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Memory
{
    public class Register_16
    {
        public bool[] X = new bool[16];
        public bool[] output = new bool[16];
        public bool cl;
        public bool st;
        DataFlipFlop []DFF= new DataFlipFlop[16];

        public Register_16()
        {
            for (int i = 0; i < 16; i++)
            {
                DFF[i]= new DataFlipFlop();
            }
        }
        public int proceed()
        {
            int val = 0;
            for(int i=0;i<16;i++)
            {
                DFF[i].d = X[i];
                DFF[i].cl= cl;
                DFF[i].st = st;
                val +=DFF[i].proceed();
                output[i] = DFF[i].output;
            }
            return val;
        }
    }
}
