using Controller.Arithmetics;
using Controller.InOut;
using Controller.LogicGates;
using Controller.Memory;
using Controller.Switching;
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
        bool debug_RamRange = false;

        Register_16 reg_a =new Register_16();
        Register_16 reg_d = new Register_16();
        public InputOutput inout = new InputOutput();
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

            //0x4000 -> 5FFF                //0100 0000 0000 0000
            //0x6000 => network or keyboard //0110 0000 0000 0000
           
            
            //--------------------------------------------------
            //0x7FF => in/out              //0111 1111 1111 1111
            bool[] _inoutadd = new bool[16];
            _inoutadd[15]=true;
            Inverter_16 inv16= new Inverter_16();
            inv16.a = reg_a.output;//100000000000000
            inv16.proceed();
            inv16.s[15] = false;
            Equal0_16 eq_0x7FFF = new Equal0_16();
            eq_0x7FFF.a = inv16.s;
            eq_0x7FFF.proceed();
            /*
            And clInOut_And = new And();
            clInOut_And.a = eq_0x7FFF.s;
            clInOut_And.b = cl;
            val += clInOut_And.proceed();
            */

            And stInOut_And = new And();
            stInOut_And.a = eq_0x7FFF.s;
            stInOut_And.b = ca;
            val += stInOut_And.proceed();

            inout.X = X;
            inout.st = stInOut_And.output;
            inout.cl = cl;//clInOut_And.output;
            val += inout.proceed();
            //CA = reg_inout.output;

            //-----------------------------------------------------
            //RAM
            //   0FFF   //0000 XXXX XXXX XXXX
            Equal0_2 eq1 = new Equal0_2();
            Equal0_2 eq2 = new Equal0_2();
            eq1.a0 = reg_a.output[15];
            eq1.a1 = reg_a.output[14];
            eq2.a0 = reg_a.output[13];
            eq2.a1 = reg_a.output[12];
            val += eq1.proceed();
            val += eq2.proceed();
            And ramRange_And= new And();
            ramRange_And.a = eq1.s;
            ramRange_And.b = eq2.s;
            val += ramRange_And.proceed();
            
            debug_RamRange = ramRange_And.output;

            And clRam_And = new And();
            clRam_And.a = ramRange_And.output;
            clRam_And.b = cl;
            val += clRam_And.proceed();


            ram.X = X;
            ram.st = ca;
            ram.cl = clRam_And.output;//cl;
            ram.ad = reg_a.output;
            val += ram.proceed();
            //CA = ram.output;

            Selector_16 selector_ram_inout_16 = new Selector_16();
            selector_ram_inout_16.D1 = ram.output;
            selector_ram_inout_16.D0 = inout.output;
            selector_ram_inout_16.s = ramRange_And.output;
            selector_ram_inout_16.proceed();
            CA= selector_ram_inout_16.output;

            return val;
        }

    }
}
