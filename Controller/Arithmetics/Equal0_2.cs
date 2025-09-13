using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Arithmetics
{
    public class Equal0_2
    {
        public bool a0;
        public bool a1;

        public bool s;
        public void proceed()
        {
            Invert inv1= new Invert();
            inv1.input = a0;
            inv1.proceed();

            Invert inv2 = new Invert();
            inv2.input = a1;
            inv2.proceed();

            And and= new And();
            and.a = inv1.output;
            and.b = inv2.output;
            and.proceed();
            s = and.output;

        }
    }
}
