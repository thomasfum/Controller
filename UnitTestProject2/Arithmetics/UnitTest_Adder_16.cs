using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject2.Arithmetics
{
    [TestClass]
    public class UnitTest_Adder_16
    {

       
        [TestMethod]
        public void TestMethod_2_5_1()
        {
            Adder_16 Adder = new Adder_16();
            Adder.a = Tools.int2BoolArray(2);
            Adder.b = Tools.int2BoolArray(5);
            Adder.c = true;
            Adder.proceed();
            int integerVal = Tools.boolArray2Int(Adder.s);
            Assert.AreEqual(8, integerVal);
            Assert.AreEqual(false, Adder.cOut);

        }
    }
}
