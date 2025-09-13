using Controller;
using Controller.Arithmetics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject2.Arithmetics
{
    [TestClass]
    public class UnitTest_Substracter_16
    {

        [TestMethod]
        public void TestMethod_5_2()
        {
            Substracter_16 Sub = new Substracter_16();
            Sub.a = Tools.int2BoolArray(5);
            Sub.b = Tools.int2BoolArray(2);
            int val=Sub.proceed();
            int integerVal = Tools.boolArray2Int(Sub.s);
            Assert.AreEqual(3, integerVal);
            Assert.AreEqual(960, val);
        }
        [TestMethod]
        public void TestMethod_m5_2()
        {
            Substracter_16 Sub = new Substracter_16();
            Sub.a = Tools.int2BoolArray(-5);
            Sub.b = Tools.int2BoolArray(2);
            int val = Sub.proceed();
            int integerVal = Tools.boolArray2Int(Sub.s);
            Assert.AreEqual(-7, integerVal);
            Assert.AreEqual(960, val);
        }
    }
}
