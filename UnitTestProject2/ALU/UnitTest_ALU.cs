using Controller;
using Controller.ALU;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.ALU
{
    [TestClass]
    public class UnitTest_ALU
    {

        void test(bool u, bool op1, bool op0, bool zx, bool sw, int X, int Y, int expected)
        {
            Controller.ALU.ALU alu = new Controller.ALU.ALU();
            alu.u = u;
            alu.op0 = op0;
            alu.op1 = op1;
            alu.zx = zx;
            alu.sw = sw;
            alu.X = Tools.int2BoolArray(X);
            alu.Y = Tools.int2BoolArray(Y);
            int val = alu.proceed();
            Assert.AreEqual(expected, Tools.boolArray2Int(alu.output));
            Assert.AreEqual(5904, val);
        }
       
        //--------------------
        //arithmetic
        [TestMethod]
        public void TestMethod_add_5_8()
        {
            test(true,false, false, false, false, 5, 8, 13);

        }
        [TestMethod]
        public void TestMethod_sub_8_5()
        {
            test(true, true, false, false, false, 8, 5, 3);
        }
        [TestMethod]
        public void TestMethod_sub_8_5_sw()
        {
            test(true, true, false, false, true, 8, 5, -3);
        }

        [TestMethod]
        public void TestMethod_sub_8_5_zx()
        {
            test(true, true, false, true, false, 8, 5, -5);
        }
        [TestMethod]
        public void TestMethod_sub_8_5_sw_zx()
        {
            test(true, true, false, true, true, 8, 5, -8);
        }

        [TestMethod]
        public void TestMethod_5_inc()
        {
            test(true, false, true, false, false, 5, 0, 6);
        }
        [TestMethod]
        public void TestMethod_5_dec()
        {
            test(true, true, true, false, false, 5, 0, 4);
        }
        //--------------------
        //logic
        [TestMethod]
        public void TestMethod_And()
        {
            test(false, false, false, false, false, 15, 14, 14);
        }

        [TestMethod]
        public void TestMethod_Or()
        {
            test(false, false, true, false, false, 15, 14, 15);
        }
        [TestMethod]
        public void TestMethod_Xor()
        {
            test(false, true, false, false, false, 7, 2, 5);
        }

        [TestMethod]
        public void TestMethod_Inver()
        {
            test(false, true, true, false, false, 2, 0, -3);
        }

      

        //tester zx et sw
        /*
            zx	sw	Effective operation
            0	0	X - Y
            0	1	Y - X
            1	0	0 - Y
            1	1	0 - X
         
         
         */
        /*
         * 
         * 
         */
    }
}
