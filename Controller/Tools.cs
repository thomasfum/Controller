using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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


        public static int getJmp(int ctrl)
        {
            bool[] Inst = new bool[16];
            bool[] control = int2BoolArray(ctrl);
            Inst[0] = control[0];
            Inst[1] = control[1];
            Inst[2] = control[2];

            //control.I[2] ->control.I[2]
            return Tools.boolArray2Int(Inst);
        }
        public static int getTarget(int ctrl)
        {
            bool[] Inst = new bool[16];
            bool[] control = int2BoolArray(ctrl);
            Inst[0] = control[3];
            Inst[1] = control[4];
            Inst[2] = control[5];

            //control.I[3] ->control.I[5]
            return Tools.boolArray2Int(Inst);
        }

        public static int getALU(int ctrl)
        {
            bool[] Inst = new bool[16];
            bool[] control = int2BoolArray(ctrl);
            Inst[0] = control[6];
            Inst[1] = control[7];
            Inst[2] = control[8];
            Inst[3] = control[9];
            Inst[4] = control[10];
            //control.I[6] ->control.I[10]
            return Tools.boolArray2Int(Inst);
        }


        public static bool get12(int ctrl)//A or A*
        {
            bool[] control = int2BoolArray(ctrl);
            return control[12];

        }
        public static bool get15(int ctrl)//0= data, 1= ALU
        {
            bool[] control = int2BoolArray(ctrl);
            return control[15];

        }
        public static string[] DstString = { "", "*A=", "D=", "D,*A=", "A=", "A,*A=", "A,D=", "A,D,*A=" };
        //                        0                 3                     7                            13           15                        20                    24                  28
        public static string[] CalString = { "D&A", "D&A", "0", "0", "D|A", "D|A", "A", "D", "D^A", "D^A", "A", "D", "~D", "~A", "-1", "-1", "D+A", "D+A", "A", "D", "D+1", "A+1", "1", "1", "D-A", "A-D", "-A", "-D", "D-1", "A-1", "-1", "-1" };
        public static string[] JmpString = { "", ";JGT", ";JEQ", ";JGE", ";JLT", ";JNE", ";JLE", ";JMP" };


    }
}
