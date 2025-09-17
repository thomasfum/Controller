using Controller.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.InOut
{


    public class InputOutput
    {
        public bool[] X = new bool[16];
        public bool[] output = new bool[16];
        public bool cl;
        public bool st;
        Register_16 reg_inout = new Register_16();
        public int proceed()
        {
            int val = 0;
            reg_inout.X = X;
            reg_inout.st = st;
            reg_inout.cl = cl;
            val += reg_inout.proceed();
            output = reg_inout.output;
            return val;
        }
   
         public int GetInOut()
        {
            reg_inout.st = false;
            reg_inout.cl = false;
            reg_inout.proceed();
            return Tools.boolArray2Int(reg_inout.output);
        }
        public void SetInOut(int val)
        {
            reg_inout.X = Tools.int2BoolArray(val);
            reg_inout.st = true;
            reg_inout.cl = true;
            reg_inout.proceed();
            
        }
    }
}
