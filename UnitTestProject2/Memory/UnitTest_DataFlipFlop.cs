using Controller.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace UnitTestProject2.Memory
{
    [TestClass]
    public class UnitTest_DataFlipFlop
    {
        [TestMethod]
        public void TestMethod1()
        {
            DataFlipFlop dff = new DataFlipFlop();

            //nothing change
            dff.cl = false;
            dff.d = true;
            dff.st = false;
            int val= dff.proceed();
            Assert.AreEqual(false, dff.output);
            Assert.AreEqual(69, val);

            //nothing change
            dff.cl = true;
            dff.d = true;
            dff.st = false;
            dff.proceed();
            Assert.AreEqual(false, dff.output);

            //nothing change, prepare set true
            dff.cl = false;
            dff.d = true;
            dff.st = true;
            dff.proceed();
            Assert.AreEqual(false, dff.output);


            //nothing change, set true
            dff.cl = true;
            dff.d = true;
            dff.st = true;
            dff.proceed();
            Assert.AreEqual(false, dff.output);


            //output true 
            dff.cl = false;
            dff.d = false;
            dff.st = false;
            dff.proceed();
            Assert.AreEqual(true, dff.output);

            //nothing change, set false
            dff.cl = true;
            dff.d = false;
            dff.st = true;
            dff.proceed();
            Assert.AreEqual(true, dff.output);


            //output false 
            dff.cl = false;
            dff.d = false;
            dff.st = false;
            dff.proceed();
            Assert.AreEqual(false, dff.output);

        }
    }
}
