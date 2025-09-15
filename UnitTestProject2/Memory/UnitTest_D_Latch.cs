using Controller.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Memory
{
    [TestClass]
    public class UnitTest_D_Latch
    {
        [TestMethod]
        public void TestMethod1()
        {

            D_Latch dLatch = new D_Latch();
            
            //set false
            dLatch.st = true;
            dLatch.d = false;
            int val = dLatch.proceed();
            Assert.AreEqual(false, dLatch.output);
            Assert.AreEqual(30, val);

            //set true
            dLatch.st = true;
            dLatch.d = true;
            dLatch.proceed();
            Assert.AreEqual(true, dLatch.output);

            //unchanged
            dLatch.st = false;
            dLatch.d = true;
            dLatch.proceed();
            Assert.AreEqual(true, dLatch.output);

            //unchanged
            dLatch.st = false;
            dLatch.d = false;
            dLatch.proceed();
            Assert.AreEqual(true, dLatch.output);

            //set false
            dLatch.st = true;
            dLatch.d = false;
            dLatch.proceed();
            Assert.AreEqual(false, dLatch.output);

            //unchanged
            dLatch.st = false;
            dLatch.d = true;
            dLatch.proceed();
            Assert.AreEqual(false, dLatch.output);

            //unchanged
            dLatch.st = false;
            dLatch.d = false;
            dLatch.proceed();
            Assert.AreEqual(false, dLatch.output);
            
        }
    }
}
