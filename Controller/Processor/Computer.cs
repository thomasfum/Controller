using Controller.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Processor
{

    public class Computer
    {
        public CombinedMemory memory ;
        Rom rom;
        Counter_16 counter = new Counter_16();
        ControlUnit control = new ControlUnit();
        public Computer(int romsize, int ramsize)
        {
            memory = new CombinedMemory(256);
            rom = new Rom(256);
        }
        int pulse(bool cl)
        {
            int val = 0;
            counter.cl= cl;
            counter.X = memory.A;
            counter.st = control.j;
            val+=counter.proceed();

            rom.ad = counter.output;
            val += rom.proceed();

            control.I= rom.output;
            control.A=memory.A;
            control.D = memory.D;
            control.CA = memory.CA;
            val += control.proceed();
            
            memory.cl= cl;
            memory.X= control.R;
            memory.a = control.a;
            memory.d = control.d;
            memory.ca = control.ca;
            val += memory.proceed();

            return val;
        }
        public void reset()
        {
            counter.cl = true;
            counter.st = true;
            counter.X = Tools.int2BoolArray(0);
            counter.proceed();
        }
        public int proceed()
        {
            int val = 0;
            val = pulse(false);
            pulse(true);
            return val;
        }
        public int getPC()
        {
            return Tools.boolArray2Int(counter.output);
        }
        public int getA()
        {
            return Tools.boolArray2Int(memory.A);
        }
        public int getD()
        {
            return Tools.boolArray2Int(memory.D);
        }
        public int getI()
        {
            return Tools.boolArray2Int(control.I);
        }


      

        public void setRom(int add, int val)
        {
            rom.InitRegister(Tools.int2BoolArray(add), Tools.int2BoolArray(val));
          
        }
        public void setRam(int add, int val)
        {
            memory.ram.InitRegister(Tools.int2BoolArray(add), Tools.int2BoolArray(val));
        }
        
        public int GetRom(int add)
        {
            return Tools.boolArray2Int(rom.GetRegister(Tools.int2BoolArray(add)));

        }
        public int GetRam(int add)
        {
            return Tools.boolArray2Int(memory.ram.GetRegister(Tools.int2BoolArray(add)));

        }

        public int GetInOut()
        {
            return memory.inout.GetInOut();
            /*
            memory.reg_inout.st = false;
            memory.reg_inout.cl = false;
            memory.reg_inout.proceed();
            return Tools.boolArray2Int(memory.reg_inout.output);
            */
        }
        public void SetInOut(int val)
        {
            memory.inout.SetInOut(val);
            /*
            memory.reg_inout.st = false;
            memory.reg_inout.cl = false;
            memory.reg_inout.proceed();
            return Tools.boolArray2Int(memory.reg_inout.output);
            */
        }
    }
}
