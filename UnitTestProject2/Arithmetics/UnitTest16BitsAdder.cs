using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class UnitTest16BitsAdder
    {

        int boolArray2Int(bool[] b_array)
        {
            int result = 0;
            int run = 0;
            for (int id = 0; id < 16; id++)
            {
                result |= b_array[id] ? (1 << run) : 0;
                run++;
            }
            return result;
        }
        bool[] int2BoolArray(int val)
        {
            return new BitArray(new int[] { val }).Cast<bool>().ToArray();
        }
        [TestMethod]
        public void TestMethod_2_5_1()
        {
            SixteenBitsAdder Adder = new SixteenBitsAdder();
            Adder.a = int2BoolArray(2);
            Adder.b = int2BoolArray(5);
            Adder.c = true;
            Adder.proceed();
            int integerVal = boolArray2Int(Adder.s);
            Assert.AreEqual(8, integerVal);
            Assert.AreEqual(false, Adder.cOut);

        }
    }
}
