using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Invert
    {
        public bool input = false;
        public bool output = false;

        public int proceed()
        {
            Nand nand= new Nand();
            nand.a = input;
            nand.b = input;
            int val=nand.proceed();
            output = nand.output;
            return val;
        }
    }
}
