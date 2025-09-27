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


        Register_16 reg_a =new Register_16();
        Register_16 reg_d = new Register_16();
        public InputOutput inout = new InputOutput();
        public Display display = new Display();
        public Keyboard keyboard = new Keyboard();
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

            Switch_4_16 switchTopCL= new Switch_4_16();
            switchTopCL.a0 = reg_a.output[12];
            switchTopCL.a1 = reg_a.output[13];
            switchTopCL.a2 = reg_a.output[14];
            switchTopCL.a3 = reg_a.output[15];
            switchTopCL.d = cl;
            val +=switchTopCL.proceed();
           
            Or orDisplay = new Or();
            orDisplay.a= switchTopCL.c[4];
            orDisplay.b = switchTopCL.c[5];
            val+=orDisplay.proceed();
           
            Switch_4_16 switchTopTrue = new Switch_4_16();
            switchTopTrue.a0 = reg_a.output[12];
            switchTopTrue.a1 = reg_a.output[13];
            switchTopTrue.a2 = reg_a.output[14];
            switchTopTrue.a3 = reg_a.output[15];
            switchTopTrue.d = true;
            val += switchTopTrue.proceed();


            //--------------------------------------------------
            //0x4000 -> 0x5FFF          //0100 0000 0000 0000  /16 384
            //0x5FFF                    //0101 1111 1111 1111
            display.X= X;
            display.A = A;
            display.st = ca;
            display.cl = orDisplay.output;
            val += display.proceed();

            //--------------------------------------------------
            //0x6000 => keyboard        //0110 0000 0000 0000
            keyboard.st = false;
            keyboard.cl = switchTopCL.c[6];
            val += keyboard.proceed();
            
            //--------------------------------------------------
            //0x7FF => in/out           //0111 1111 1111 1111
            inout.X = X;
            inout.st = ca;
            inout.cl = switchTopCL.c[7];
            val += inout.proceed();

            //-----------------------------------------------------
            //0x0000 -> 0x0FFF =>RAM    //0000 XXXX XXXX XXXX
            ram.X = X;
            ram.st = ca;
            ram.cl = switchTopCL.c[0];
            ram.ad = reg_a.output;
            val += ram.proceed();

            //-----------------------------------------------------
            //output
            Selector_16 selector_ram_inout_16 = new Selector_16();
            selector_ram_inout_16.D1 = ram.output;
            selector_ram_inout_16.D0 = inout.output;
            selector_ram_inout_16.s = switchTopTrue.c[0];//ram
            val+=selector_ram_inout_16.proceed();

            Selector_16 selector_ram_inout_16_Keyboard = new Selector_16();
            selector_ram_inout_16_Keyboard.D1 = keyboard.output;
            selector_ram_inout_16_Keyboard.D0 = selector_ram_inout_16.output;
            selector_ram_inout_16_Keyboard.s = switchTopTrue.c[6];// Keyboard;
            val += selector_ram_inout_16_Keyboard.proceed();
            CA = selector_ram_inout_16_Keyboard.output;

            return val;
        }

    }
}
