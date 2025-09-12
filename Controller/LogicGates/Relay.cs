
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Relay
    {
        bool defaultOff=true;
        public bool c;
        public bool input;
        public bool output;
        public Relay(bool _defaultOff)
        {
            defaultOff = _defaultOff;
        }
        public void proceed()
        {
            if (defaultOff==true)
            {
                if(c==true)  
                    output = input;
                else
                    output = false;
            }
            else
            {
                if (c == false)
                    output = input;
                else
                    output = false;
            }
        }
    }
}
