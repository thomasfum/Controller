using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Memory
{
    public class D_Latch
    {
        public bool st;
        public bool d;
        public bool output;
        SR_Latch srLatch = new SR_Latch();
        Invert invSt = new Invert();
        Invert invD = new Invert();
        Or or1 = new Or();
        Or or2 = new Or();

        public int proceed()
        {
            int val = 0;

            invD.input = d;
            val+= invD.proceed();

            invSt.input = st;
            val += invSt.proceed();

            or1.a = invSt.output;
            or1.b = d;
            val += or1.proceed();

            or2.a = invD.output;
            or2.b = invSt.output;
            val += or2.proceed();

            srLatch.s = or1.output;
            srLatch.r = or2.output;
            val += srLatch.proceed();
            output = srLatch.output;

            return val;
        }
    }
}
