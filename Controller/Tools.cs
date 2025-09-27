using Controller.ALU;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            for (int id = 0; id < b_array.Length; id++)
            {
                result |= b_array[id] ? (1 << run) : 0;
                run++;
            }
            /*
            if (result <0 )
                return  65536 + result;

            if (result > 65536 / 2)
                return result - 65536;
            else
            */
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
        static string getDststr(int I)
        {
            return DstString[getTarget(I)];
        }
        static string getALUstr(int I)
        {
            string s = CalString[getALU(I)];
            if (get12(I) == true)
            {
                return s.Replace("A", "*A");
            }
            return s;
        }
        static string getJMPstr(int I)
        {
            return JmpString[getJmp(I)];
        }
        public static string GetInstruction(int I)
        {
            string a = getDststr(I) + getALUstr(I) + getJMPstr(I);
            return a;
        }
        static string[] DstString = { "", "*A=", "D=", "D,*A=", "A=", "A,*A=", "A,D=", "A,D,*A=" };
        //                              0                 3                     7                                   13           15                           20                      24                        28
        static string[] CalString = { "D&A", "D&A", "0", "0", "D|A", "D|A", "A", "D", "D^A", "D^A", "A", "D", "~D", "~A", "-1", "-1", "D+A", "D+A", "A", "D", "D+1", "A+1", "1", "1", "D-A", "A-D", "-A", "-D", "D-1", "A-1", "-1", "-1" };
        static string[] JmpString = { "", ";JGT", ";JEQ", ";JGE", ";JLT", ";JNE", ";JLE", ";JMP" };


        static readonly Regex binary = new Regex("^[01]{1,32}$", RegexOptions.Compiled);


        public static int ParseNumber(string number, out string Msg)
        {
            int n;
            Msg = "";
            bool isNumeric = int.TryParse(number, out n);
            if (!isNumeric)
            {
                if (number.StartsWith("'"))
                {
                    if (number.Length != 3)
                    {
                        Msg = "Wrong char lengh";
                        return 0;
                    }
                    if(!number.EndsWith("'"))
                    {
                        Msg = "char should finich by '";
                        return 0;
                    }
                    return number[1];
                }
                number= number.ToUpper();
                if (number.StartsWith("0X")|| number.StartsWith("X"))
                {
                    char[] _trim_hex = new char[] { '0', 'X' };
                    int temp;
                    if (int.TryParse(number.TrimStart(_trim_hex), NumberStyles.HexNumber, null, out temp))
                    {
                        return temp;
                    }
                    else
                    {
                        Msg = "is not an hexa number";
                        return 0;
                    }
                }
                else if (number.StartsWith("0B")|| number.StartsWith("B"))
                {
                    if (binary.IsMatch(number.Substring(2)))
                    {
                        return Convert.ToInt32(number.Substring(2), 2) ;
                    }
                    else
                    {
                        Msg = "is not a binary number";
                        return 0;
                    }
                }
                else
                {
                    Msg = "is not a number";
                    return 0;
                }
            }
            else
                return n;
        
        }
        public static int Decode(string _val, Dictionary<string, int> Csts, out string Msg)
        {
            string val=_val.Replace(" ","");
            if (val=="")
            { 
                Msg = "Empty value";
                return 0;
            }
            Msg = "";
            if (val.ToUpper().StartsWith("A="))
            {

                string number = val.Substring(2);
                if (Csts.ContainsKey(number.ToUpper()))
                {
                    return Csts[number];
                }
                else
                {
                    int val1=ParseNumber(number, out Msg);
                    if (Msg != "")
                    {

                        // Msg = "Value for A " + Msg;
                        //  return 0;
                        Msg = "";//can be *A, D
                    }
                    else
                        return val1;
                }
            }


            int top = 0;
            int operation = 0;
            int target = 0;
            int jmp = 0;
            bool needstar = false;
            val=val.ToUpper();

            if (val.StartsWith("*A="))
            {
                needstar = true;
            }
            string DST = "";
            string OP = "";
            string JMP = "";
            string []split=val.ToUpper().Split( '=' );
            if (split.Length > 1)
            {
                string[] split2 = split[1].Split(';');
                DST = split[0] + "=";
                OP = split2[0];
                JMP = "";
                if (split2.Count() > 1)
                    JMP = split2[1];
            }
            else
            {
                string[] split3 = val.Split(';');
                if (split3.Length > 1)
                {
                    OP = split3[0];
                    JMP = "";
                    if (split3.Count() > 1)
                        JMP = split3[1];
                }
                else //a single command
                {
                    if (val == "JMP")
                        JMP = val;
                }
            }


            //Jmp
            bool JmpOk = false;
            for (int i = 0; i < JmpString.Count(); i++)
            {
                string st = JmpString[i].Replace(";","");
                if (JMP == st)
                {
                    jmp = i;
                    JmpOk = true;
                    break;
                }
            }
            if (JmpOk != true)
            {
                Msg = "Wrong JMP";
                return 0;
            }

            //Destination
            bool DstOk = false;
            //for (int i = 1; i < DstString.Count(); i++)
            for (int i = 0; i < DstString.Count(); i++)
            {
                string st = DstString[i];
                if (DST == st)
                {
                    target = i;
                    DstOk = true;
                    break;
                }
            }
            if (DstOk != true)
            {
                Msg = "Wrong Destination";
                return 0;
            }
            //Calc
            if (OP != "")
            {
                bool CalcOk = false;
                if (OP.Contains("*"))
                {
                    needstar = true;
                    OP=OP.Replace("*","");
                }
           
                //for (int i = 1; i < CalString.Count(); i++)
                for (int i = 0; i < CalString.Count(); i++)
                {
                    string st = CalString[i];
                    if (OP == st)
                    {
                        operation = i;
                        CalcOk = true;
                        break;
                    }
                }
                if (CalcOk != true)
                {
                    Msg = "Wrong Calculation";
                    return 0;
                }
            }
           
            //top
            top = 16;
            if(needstar == true)
                top = top | 2;

            int dword = jmp + (target * 8) + (operation * 64) + (top *2048);
            if (dword < 0)
                dword = 65535 - dword+1;
            return dword;//-31472
        }
    }
}
