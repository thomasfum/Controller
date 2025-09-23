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
        public int proceed()
        {
            int val = 0;
            if (st == true)
            {
                int key = keyboardCallback();
                output = Tools.int2BoolArray(key);
                Console.WriteLine("keboard" + key);
            }
            return val;
        }

        public void setKeyboardCallBack(Func<int> _keyboardCallback)
        {
            keyboardCallback = _keyboardCallback;
        }
    }
}
