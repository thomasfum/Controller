using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Memory
{
    public class Rom
    {
        public bool[] ad = new bool[16];
        public bool[] output = new bool[16];
        public bool cl;
        Register_16[] regsiters;

        public Rom(int _nbRegister)
        {
            regsiters = new Register_16[_nbRegister];
            for (int i = 0; i < _nbRegister; i++)
                regsiters[i] = new Register_16();
        }
        public int proceed()
        {
            bool[] add = new bool[65638];
            add[Tools.boolArray2Int(ad)] = true;//TODO decodage is missing
            int val = 0;
            for (int i = 0; i < regsiters.Count(); i++)
            {
                regsiters[i].cl = cl;
                regsiters[i].st = add[i];
                regsiters[i].proceed();
            }
            output = regsiters[Tools.boolArray2Int(ad)].output;//TODO decodage is missing
            return val;
        }
        public void InitRegister(bool[] ad, bool[] val)
        {
            int add = Tools.boolArray2Int(ad);
            regsiters[add].X = val;
            regsiters[add].cl = true;
            regsiters[add].st = true;
            regsiters[add].proceed();
            regsiters[add].cl = false;
            regsiters[add].proceed();
        }

        public bool[] GetRegister(bool[] ad)
        {
            int add = Tools.boolArray2Int(ad);
            regsiters[add].cl = true;
            regsiters[add].st = false;
            regsiters[add].proceed();
            regsiters[add].cl = false;
            regsiters[add].proceed();
            return regsiters[add].output;
        }
    }
}
