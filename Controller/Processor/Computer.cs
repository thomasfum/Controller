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
        CombinedMemory memory ;
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
        public void setRom(int add, int val)
        {
            rom.InitRegister(Tools.int2BoolArray(add), Tools.int2BoolArray(val));
          
        }

    }
}
