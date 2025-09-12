using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class And
    {

        public bool a;
        public bool b;
        public bool output;

        public void proceed()
        {
            Nand nand= new Nand();
            nand.a = a; nand.b=b;
            nand.proceed();
            Invert invert= new Invert();
            invert.input = nand.output;
            invert.proceed();
            output =invert.output;
        }
    }
}
