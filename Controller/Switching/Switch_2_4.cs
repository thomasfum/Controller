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
        public int proceed()
        {
            int val = 0;
            /*
            Invert invA0 = new Invert();
            invA0.input = a0;
            val += invA0.proceed();

            Invert invA1 = new Invert();
            invA1.input = a1;
            val += invA1.proceed();

            And andD0 = new And();
            andD0.a = invA0.output;
            andD0.b = invA1.output;
            val += andD0.proceed();
            d[0] = andD0.output;

            And andD1 = new And();
            andD1.a = a0;
            andD1.b = invA1.output;
            val += andD1.proceed();
            d[1] = andD1.output;

            And andD2 = new And();
            andD2.a = invA0.output;
            andD2.b = a1;
            val += andD2.proceed();
            d[2] = andD2.output;

            And andD3 = new And();
            andD3.a = a0;
            andD3.b = a1;
            val += andD3.proceed();
            d[3] = andD3.output;
            */


            Switch_1_2 sw1= new Switch_1_2();
            sw1.d = d;
            sw1.a = a1;
            val += sw1.proceed();

            Switch_1_2 sw2 = new Switch_1_2();
            sw2.d = sw1.c0;
            sw2.a = a0;
            val += sw2.proceed();

            Switch_1_2 sw3 = new Switch_1_2();
            sw3.d = sw1.c1;
            sw3.a = a0;
            val += sw3.proceed();

            c[0] = sw2.c0;
            c[1] = sw2.c1;
            c[2] = sw3.c0;
            c[3] = sw3.c1;

            return val;
        }
    }
}
