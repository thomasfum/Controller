using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public  class Tools
    {
        public static int boolArray2Int(bool[] b_array)
        {
            int result = 0;
            int run = 0;
            for (int id = 0; id < 16; id++)
            {
                result |= b_array[id] ? (1 << run) : 0;
                run++;
            }
            if (result > 65536 / 2)
                return result - 65536;
            else
                return result;
        }
        public static bool[] int2BoolArray(int val)
        {
            //if (val < 0)
            //    val += 65536;
            return new BitArray(new int[] { val }).Cast<bool>().ToArray();
        }
    }
}
