using Controller;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest_Tools
    {
        [TestMethod]
        public void TestMethod_Get12()
        {
            bool val_1=Tools.get12(4096);
            Assert.IsTrue(val_1);
            bool val_0 = Tools.get12(-4097);
            Assert.IsFalse(val_0);
        }

        [TestMethod]
        public void TestMethod_Get15()
        {
            bool val_1 = Tools.get15(32768);
            Assert.IsTrue(val_1);
            bool val_0 = Tools.get15(-32769);
            Assert.IsFalse(val_0);
        }
    }
}
