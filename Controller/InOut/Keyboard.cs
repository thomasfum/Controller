using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller.InOut
{
    public class Keyboard
    {
        Func<int> keyboardCallback;
        public bool[] X = new bool[16];
        public bool[] output = new bool[16];
        public bool cl;
        public bool st;
        int lastKey = 0;
        public int proceed()
        {
            int val = 0;
            if (cl == true)
            {
                int key = keyboardCallback();
                //if (lastKey == 0)
                    output = Tools.int2BoolArray(key);
                //else
                //    output = Tools.int2BoolArray(0);
                lastKey = key;

                if (key!=0)
                    Console.WriteLine("keboard " + key +" => "+ (char)key);

            }
            return val;
        }

        public void setKeyboardCallBack(Func<int> _keyboardCallback)
        {
            keyboardCallback = _keyboardCallback;
        }
    }
}
