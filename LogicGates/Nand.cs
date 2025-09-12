using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Nand
    {
        Relay relay1;
        Relay relay2;
        Relay relay3;
        public bool a;
        public bool b;
        public bool output;
        public Nand()
        {
            relay1 = new Relay(true);//default off
            relay2 = new Relay(true);//default off
            relay3 = new Relay(false);//default off
        }
        public void proceed()
        {
            relay1.input = true;
            relay1.c = b;
            relay1.proceed();
            relay2.input = relay1.output;
            relay2.c = a;
            relay2.proceed();
            relay3.input = true;
            relay3.c = relay2.output;
            relay3.proceed();
            output = relay3.output;
        }
    }
}
