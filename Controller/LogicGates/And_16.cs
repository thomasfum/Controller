using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.LogicGates
{

    public class And_16
    {
        public bool[] a = new bool[16];
        public bool[] b = new bool[16];
        public bool[] s = new bool[16];

        public int proceed()
        {
            int val = 0;
            for (int i = 0; i < 16; i++)
            {
                And and = new And();
                and.a = a[i];
                and.b = b[i];
                val += and.proceed();
                s[i] = and.output;
            }
            return val;
        }
    }
}
