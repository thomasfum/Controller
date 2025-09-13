using Controller;
using Controller.Arithmetics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Arithmetics
{
    [TestClass]
    public class UnitTest_Less0_16
    {
        [TestMethod]
        public void TestMethod_0()
        {
            Less0_16 less0= new Less0_16();
            less0.a = Tools.int2BoolArray(0);
            int val=less0.proceed();
            Assert.AreEqual(false, less0.s);
            Assert.AreEqual(0, val);
        }
        [TestMethod]
        public void TestMethod_1()
        {
            Less0_16 less0 = new Less0_16();
            less0.a = Tools.int2BoolArray(1);
            int val = less0.proceed();
            Assert.AreEqual(false, less0.s);
            Assert.AreEqual(0, val);
        }
        [TestMethod]
        public void TestMethod_m1()
        {
            Less0_16 less0 = new Less0_16();
            less0.a = Tools.int2BoolArray(-1);
            int val = less0.proceed();
            Assert.AreEqual(true, less0.s);
            Assert.AreEqual(0, val);
        }
        [TestMethod]
        public void TestMethod_FF()
        {
            Less0_16 less0 = new Less0_16();
            less0.a = Tools.int2BoolArray(65535);
            int val = less0.proceed();
            Assert.AreEqual(true, less0.s);
            Assert.AreEqual(0, val);
        }
    }
}
