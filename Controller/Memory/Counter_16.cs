using Controller.Arithmetics;
using Controller.Switching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Memory
{
    public class Counter_16
    {
        public bool[] X = new bool[16];
        public bool[] output = new bool[16];
        public bool cl;
        public bool st;
        Register_16 reg = new Register_16();
        public int proceed()
        {
            int val = 0;
            Selector_16 sel= new Selector_16();
            Increment_16 inc = new Increment_16();


            inc.a = reg.output;
            val+=inc.proceed();


            sel.s = st;
            sel.D0 = inc.output;
            sel.D1 = X;
            val += sel.proceed();


            reg.X = sel.output;
            reg.st = cl;
            reg.cl= cl;
            val += reg.proceed();

            output=reg.output;

            return val;
        }
    }
}
