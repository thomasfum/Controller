using Controller.Arithmetics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Memory
{
    public class DataFlipFlop
    {
        public bool st;
        public bool d;
        public bool cl;
        public bool output;
        D_Latch DLatch1 = new D_Latch();
        D_Latch DLatch2    = new D_Latch();
        public int proceed()
        {
            int val = 0;
            And and= new And();
            Invert invert = new Invert();

            invert.input = cl;
            val +=invert.proceed();

            and.a= cl;
            and.b = st;
            val += and.proceed();

            DLatch1.d = d;
            DLatch1.st=and.output;
            val += DLatch1.proceed();

            DLatch2.d = DLatch1.output;
            DLatch2.st = invert.output;
            val += DLatch2.proceed();

            output = DLatch2.output;

            return val;
        }
    }
}
