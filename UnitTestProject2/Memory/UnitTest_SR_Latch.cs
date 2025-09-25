using Controller.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject2.Memory
{
    [TestClass]
    public class UnitTest_SR_Latch
    {
        [TestMethod]
        public void TestMethod1()
        {
            SR_Latch sR_Latch = new SR_Latch();
            sR_Latch.s = true;
            sR_Latch.r = false;
            int val = sR_Latch.proceed();
            Assert.AreEqual(true, sR_Latch.output);
            Assert.AreEqual(2, val);


            sR_Latch.s = true;
            sR_Latch.r = true;
            sR_Latch.proceed();
            Assert.AreEqual(true, sR_Latch.output);


            sR_Latch.s = false;
            sR_Latch.r = true;
            sR_Latch.proceed();
            Assert.AreEqual(false, sR_Latch.output);


            sR_Latch.s = true;
            sR_Latch.r = true;
            sR_Latch.proceed();
            Assert.AreEqual(false, sR_Latch.output);

            sR_Latch.s = true;
            sR_Latch.r = true;
            sR_Latch.proceed();
            Assert.AreEqual(false, sR_Latch.output);


            sR_Latch.s = true;
            sR_Latch.r = false;
            sR_Latch.proceed();
            Assert.AreEqual(true, sR_Latch.output);

            sR_Latch.s = true;
            sR_Latch.r = true;
            sR_Latch.proceed();
            Assert.AreEqual(true, sR_Latch.output);


            sR_Latch.s = true;
            sR_Latch.r = true;
            sR_Latch.proceed();
            Assert.AreEqual(true, sR_Latch.output);


        }
    }
}
