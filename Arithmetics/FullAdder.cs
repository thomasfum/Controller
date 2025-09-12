using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Arithmetics
{
    public class FullAdder
    {

        public bool a;
        public bool b;
        public bool c;
        public bool h;
        public bool l;

        public void proceed()
        {
            HalfAdder halfAdder1 = new HalfAdder();
            halfAdder1.a = b;
            halfAdder1.b = c;
            halfAdder1.proceed();


            HalfAdder halfAdder2 = new HalfAdder();
            halfAdder2.a = a;
            halfAdder2.b = halfAdder1.l;
            halfAdder2.proceed();
            l = halfAdder2.l;

            Or or = new Or();
            or.a = halfAdder1.h;
            or.b = halfAdder2.h;
            or.proceed();
            h = or.output;


        }
    }
}
