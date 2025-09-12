using DevExpress.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Or
    {
        public bool a;
        public bool b;
        public bool output;

        public void proceed()
        {
            Invert inv1= new Invert();
            inv1.input = a;
            inv1.proceed();

            Invert inv2 = new Invert();
            inv2.input = b;
            inv2.proceed();

            Nand nand = new Nand();
            nand.a = inv1.output; 
            nand.b = inv2.output;
            nand.proceed();
            output = nand.output;
        }
    }
}
