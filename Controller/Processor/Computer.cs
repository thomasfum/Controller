using Controller.Memory;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
        int NbNand_Rom = 0;
        int NbNand_Memory = 0;
        int NbNand_ControlUnit = 0;
        int NbNand_PC = 0;

        public Computer(int romsize, int ramsize)
        {
            memory = new CombinedMemory(ramsize);
            rom = new Rom(romsize);
        }
        int pulse(bool cl)
        {
            counter.cl= cl;
            counter.X = memory.A;
            counter.st = control.j;
            NbNand_PC = counter.proceed();

            rom.ad = counter.output;
            NbNand_Rom = rom.proceed();

            int debug_instucion=Tools.boolArray2Int(rom.output);
            control.I= rom.output;
            control.A=memory.A;
            control.D = memory.D;
            control.CA = memory.CA;
            NbNand_ControlUnit = control.proceed();

            int debug_memory_X = Tools.boolArray2Int(control.R);
            memory.cl= cl;
            memory.X= control.R;
            memory.a = control.a;
            memory.d = control.d;
            memory.ca = control.ca;
            NbNand_Memory = memory.proceed();

            return NbNand_PC+NbNand_Rom + NbNand_ControlUnit+ NbNand_Memory;
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
       
        //-----------------------------------------------------
        //Registries
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
        public string GetNandCount()
        {
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            int total = NbNand_PC + NbNand_Rom + NbNand_ControlUnit + NbNand_Memory;
            return "Total=" + total.ToString("#,0", nfi) + "; PC=" + NbNand_PC.ToString("#,0", nfi) + "; Ctrl=" + NbNand_ControlUnit.ToString("#,0", nfi) + "; Mem=" + NbNand_Memory.ToString("#,0", nfi) + "; Rom=" + NbNand_Rom.ToString("#,0", nfi);
        }
        //-----------------------------------------------------
        //ROM
        public void setRom(int add, int val)
        {
            rom.InitRegister(Tools.int2BoolArray(add), Tools.int2BoolArray(val));
        }
        public int GetRom(int add)
        {
            return Tools.boolArray2Int(rom.GetRegister(Tools.int2BoolArray(add)));
        }
        public void ClearRom()
        {
            rom.Clear();
        }
        //-----------------------------------------------------
        //RAM
        public void setRam(int add, int val)
        {
            memory.ram.InitRegister(Tools.int2BoolArray(add), Tools.int2BoolArray(val));
        }
        public int GetRam(int add)
        {
            return Tools.boolArray2Int(memory.ram.GetRegister(Tools.int2BoolArray(add)));
        }
        //-----------------------------------------------------
        //Inout
        public int GetInOut()
        {
            return memory.inout.GetInOut();
        }
        public void SetInOut(bool[] val)
        {
            memory.inout.SetInOut(val);
        }
        public void ResetInOut()
        {
            memory.inout.ResetInOut();
        }
        //-----------------------------------------------------
        //Display
        public void SetDisplayImage(Bitmap img)
        {
            memory.display.setImage(img);
        }
        public void setKeyboardCallBack(Func<int> _setKkeyboardCallback, Func<int> _resetKkeyboardCallback)
        {
            memory.keyboard.setKeyboardCallBack(_setKkeyboardCallback, _resetKkeyboardCallback);
        }
        public void ClearDisplay()
        {
            memory.display.Clear();
        }
    }
}
