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
            invert.proceed();
            Assert.AreEqual(false, invert.output);
        }
        [TestMethod]
        public void TestMethod_0()
        {
            Invert invert = new Invert();
            invert.input = false;
            invert.proceed();
            Assert.AreEqual(true, invert.output);
        }
    }
}
