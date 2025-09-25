using Controller.Arithmetics;
using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject2.Arithmetics
{
    [TestClass]
    public class UnitTest_Equal0_2
    {
        [TestMethod]
        public void TestMethod_0()
        {
            Equal0_2 eq = new Equal0_2();
            eq.a0=false;
            eq.a1 = false;
            int val=eq.proceed();
            Assert.AreEqual(true, eq.s);
            Assert.AreEqual(4, val);
        }
        [TestMethod]
        public void TestMethod_1()
        {
            Equal0_2 eq = new Equal0_2();
            eq.a0 = true;
            eq.a1 = false;
            int val = eq.proceed();
            Assert.AreEqual(false, eq.s);
            Assert.AreEqual(4, val);
        }
        [TestMethod]
        public void TestMethod_2()
        {
            Equal0_2 eq = new Equal0_2();
            eq.a0 = false;
            eq.a1 = true;
            int val = eq.proceed();
            Assert.AreEqual(false, eq.s);
            Assert.AreEqual(4, val);
        }
        [TestMethod]
        public void TestMethod_3()
        {
            Equal0_2 eq = new Equal0_2();
            eq.a0 = true;
            eq.a1 = true;
            int val = eq.proceed();
            Assert.AreEqual(false, eq.s);
            Assert.AreEqual(4, val);
        }
    }
}
