using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject2.LogicGates
{
    [TestClass]
    public class UnitTest_Invert
    {
        [TestMethod]
        public void TestMethod_1()
        {
            Invert invert = new Invert();
            invert.input = true;
            int val=invert.proceed();
            Assert.AreEqual(false, invert.output);
            Assert.AreEqual(1, val);
        }
        [TestMethod]
        public void TestMethod_0()
        {
            Invert invert = new Invert();
            invert.input = false;
            int val = invert.proceed();
            Assert.AreEqual(true, invert.output);
            Assert.AreEqual(1, val);
        }
    }
}
