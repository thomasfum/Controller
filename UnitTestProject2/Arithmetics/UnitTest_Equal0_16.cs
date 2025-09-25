using Controller;
using Controller.Arithmetics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Arithmetics
{
    [TestClass]
    public class UnitTest_Equal0_16

    {
        [TestMethod]
        public void TestMethod_0()
        {
            Equal0_16 eq = new Equal0_16();
            eq.a= Tools.int2BoolArray(0);
            int val=eq.proceed();
            Assert.AreEqual(true, eq.s);
            Assert.AreEqual(46, val);
        }
        [TestMethod]
        public void TestMethod_FF()
        {
            Equal0_16 eq = new Equal0_16();
            eq.a = Tools.int2BoolArray(65535);
            int val = eq.proceed();
            Assert.AreEqual(false, eq.s);
            Assert.AreEqual(46, val);
        }
    }
}
