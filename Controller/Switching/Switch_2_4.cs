using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Switching
{
    public class Switch_2_4
    {
        //https://www.elprocus.com/designing-3-line-to-8-line-decoder-demultiplexer/
        //https://www.researchgate.net/figure/18-DeMux-using-12-DeMux-circuits_fig11_351679182
        public bool a0;
        public bool a1;
        public bool d;
        public bool[] c = new bool[4];
        Switch_1_2 sw1 = new Switch_1_2();
        Switch_1_2 sw2_1 = new Switch_1_2();
        Switch_1_2 sw2_2 = new Switch_1_2();
        public int proceed()
        {
            int val = 0;
          
            sw1.d = d;
            sw1.a = a1;
            val += sw1.proceed();
                        
            sw2_1.d = sw1.c0;
            sw2_1.a = a0;
            val += sw2_1.proceed();

            sw2_2.d = sw1.c1;
            sw2_2.a = a0;
            val += sw2_2.proceed();

            c[0] = sw2_1.c0;
            c[1] = sw2_1.c1;
            c[2] = sw2_2.c0;
            c[3] = sw2_2.c1;

            return val;
        }
    }
}
