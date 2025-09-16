using Controller.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Processor
{
    public class CombinedMemory
    {
        public bool[] X = new bool[16];
        public bool[] A = new bool[16];
        public bool[] D = new bool[16];
        public bool[] CA = new bool[16];
        public bool cl;
        public bool a;
        public bool d;
        public bool ca;

        Register_16 reg_a =new Register_16();
        Register_16 reg_d = new Register_16();
        public Ram ram;
        public CombinedMemory(int _nbRegister)
        {
            ram = new Ram(_nbRegister);
        }
        public int proceed()
        {

            int val = 0;
            reg_a.X= X;
            reg_a.st = a;
            reg_a.cl = cl;
            val+= reg_a.proceed();
            A = reg_a.output;

            reg_d.X = X;
            reg_d.st = d;
            reg_d.cl = cl;
            val += reg_d.proceed();
            D = reg_d.output;

            ram.X = X;
            ram.st = ca;
            ram.cl = cl;
            ram.ad = reg_a.output;
            val += ram.proceed();
            CA = ram.output;

            return val;
        }

    }
}
