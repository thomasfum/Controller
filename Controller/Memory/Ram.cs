using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.Memory
{
    public class Ram
    {
        public bool[] X = new bool[16];
        public bool[] ad = new bool[16];
        public bool[] output = new bool[16];
        public bool cl;
        public bool st;
        Register_16[] regsiters;
        public Ram(int _nbRegister)
        {
            regsiters= new Register_16[_nbRegister];
            for(int i=0;i< _nbRegister;i++)
                regsiters[i]= new Register_16();
        }
        public int proceed()
        {
            bool [] add = new bool [65638];
            add[Tools.boolArray2Int(ad)] = true;//TODO decodage is missing
            int val = 0;
            for (int i = 0; i < regsiters.Count(); i++)
            {
                And and= new And();
                and.a = add[i];
                and.b = st;
                val+=and.proceed();
                regsiters[i].X = X;
                regsiters[i].cl = cl;
                regsiters[i].st = and.output;
                regsiters[i].proceed();
            }
            output = regsiters[Tools.boolArray2Int(ad)].output;//TODO decodage is missing
            return val;
        }
    }
}
