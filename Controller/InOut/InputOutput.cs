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
        Register_16 reg_in = new Register_16();
        Register_16 reg_out = new Register_16();
        public int proceed()
        {
            int val = 0;
            if (st == true)//write to out
            {
                reg_out.X = X;
                reg_out.st = st;
                reg_out.cl = cl;
                val += reg_out.proceed();
                output = reg_out.output;
              
            }
            else//read
            {
                reg_in.X = X;
                reg_in.st = st;
                reg_in.cl = cl;
                val += reg_in.proceed();
                output = reg_in.output;
            }
            return val*2;
        }

        public int GetInOut()
        {
            reg_out.st = false;
            reg_out.cl = false;
            reg_out.proceed();
            return Tools.boolArray2Int(reg_out.output);
        }
        public void SetInOut(bool[] val)
        {
            reg_in.X = val;
            reg_in.st = true;
            reg_in.cl = true;
            reg_in.proceed();

        }

        public void ResetInOut()
        {
            reg_out.X = Tools.int2BoolArray(0);
            reg_out.st = true;
            reg_out.cl = true;
            reg_out.proceed();
        }

        /*
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
            if (st == true)
            {
                int toto = 55;
            }
            reg_inout.cl = cl;
            val += reg_inout.proceed();
            output[0] = reg_inout.output[8];
            output[1] = reg_inout.output[9];
            output[2] = reg_inout.output[10];
            output[3] = reg_inout.output[11];
            output[4] = reg_inout.output[12];
            output[5] = reg_inout.output[13];
            output[6] = reg_inout.output[14];
            output[7] = reg_inout.output[15];
            output[8] = false;
            output[9] = false;
            output[10] = false;
            output[11] = false;
            output[12] = false;
            output[13] = false;
            output[14] = false;
            output[15] = false;

            return val;
        }
   
         public int GetInOut()
        {
            reg_inout.st = false;
            reg_inout.cl = false;
            reg_inout.proceed();
            return Tools.boolArray2Int(reg_inout.output);
        }
        public void SetInOut(bool[]val)
        {
            reg_inout.X = val;
            reg_inout.st = true;
            reg_inout.cl = true;
            reg_inout.proceed();
            
        }
        */
    }
}
