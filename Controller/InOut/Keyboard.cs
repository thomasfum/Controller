using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Controller.InOut
{
    public class Keyboard
    {
        Func<int> keyboardCallback;
        public bool[] X = new bool[16];
        public bool[] output = new bool[16];
        public bool cl;
        public bool st;
        int lastkey = 0;
        int cptRead = 1;
        public int proceed()
        {
            int val = 0;
            if (cl == true)
            {
               
                if(cptRead==1)
                    lastkey = keyboardCallback();
                if (lastkey != 0)
                    Console.WriteLine("keboard " + lastkey + " => "+ (char)lastkey);
                cptRead++;
                if(cptRead == 2)
                    cptRead = 0;
                output = Tools.int2BoolArray(lastkey);

            }
            
            if (cl == false)
            {
               output= Tools.int2BoolArray(lastkey);
            }
            
            return val;
        }

        public void setKeyboardCallBack(Func<int> _keyboardCallback)
        {
            keyboardCallback = _keyboardCallback;
        }
    }
}
