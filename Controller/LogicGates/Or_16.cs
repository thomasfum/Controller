using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.LogicGates
{
    public class Or_16
    {
        public bool[] a = new bool[16];
        public bool[] b = new bool[16];
        public bool[] s = new bool[16];
        Or[] or = new Or[16];
        public Or_16()
        {
            for (int i = 0; i < 16; i++)
                or[i] = new Or();
        }
        public int proceed()
        {
            int val = 0;
            for (int i = 0; i < 16; i++)
            {
                or[i].a = a[i];
                or[i].b = b[i];
                val += or[i].proceed();
                s[i] = or[i].output;
            }
            return val;
        }
    }
}
