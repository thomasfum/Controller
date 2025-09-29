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
        Func<int> setKeyboardCallback;
        Func<int> resetKeyboardCallback;
        public bool[] X = new bool[16];
        public bool[] output = new bool[16];
        public bool cl;
        public bool st;
        int lastkey = 0;
        public int proceed()
        {
            int val = 0;
            if ((cl == true)&&(st==false))
            {
                output = Tools.int2BoolArray(lastkey);
            }
            if ((cl == true) && (st == true))
            {
                output = Tools.int2BoolArray(lastkey);
                resetKeyboardCallback();
            }
            if (cl == false)
            {
                output = Tools.int2BoolArray(lastkey);
                lastkey = setKeyboardCallback();
              
                //if (lastkey != 0)
                //    Console.WriteLine("keboard " + lastkey + " => " + (char)lastkey);
            }
            return val;
        }

        public void setKeyboardCallBack(Func<int> _setKkeyboardCallback, Func<int> _resetKkeyboardCallback )
        {
            setKeyboardCallback = _setKkeyboardCallback;
            resetKeyboardCallback = _resetKkeyboardCallback;
        }
    }
}
