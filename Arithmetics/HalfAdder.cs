using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Arithmetics
{
    public class HalfAdder
    {

        public bool a;
        public bool b;
        public bool h;
        public bool l;
        public void proceed()
        {
            Xor xor = new Xor();
            xor.a = a;
            xor.b = b;
            xor.proceed();
            l = xor.output;

            And and = new And();
            and.a = a;
            and.b = b;
            and.proceed();
            h = and.output;

            
        }
    }
}
