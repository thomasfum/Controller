using Controller;
using Controller.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace UnitTestProject2.Memory
{
    [TestClass]
    public class UnitTestRam
    {
       
        [TestMethod]
        public void TestMethod1()
        {
            Ram ram= new Ram(64);
            
            ram.ad = Tools.int2BoolArray(0);
            ram.X = Tools.int2BoolArray(44);
            ram.st = true;
            ram.cl = false;
            int val=ram.proceed();
            Assert.AreEqual(0, Tools.boolArray2Int(ram.output));
            Assert.AreEqual(23680, val);

            ram.ad = Tools.int2BoolArray(0);
            ram.X = Tools.int2BoolArray(44);
            ram.st = true;
            ram.cl = true;
            ram.proceed();
            Assert.AreEqual(0, Tools.boolArray2Int(ram.output));

            ram.ad = Tools.int2BoolArray(0);
            ram.X = Tools.int2BoolArray(33);
            ram.st = false;
            ram.cl = false;
            ram.proceed();
            Assert.AreEqual(44, Tools.boolArray2Int(ram.output));

            ram.ad = Tools.int2BoolArray(1);
            ram.X = Tools.int2BoolArray(33);
            ram.st = false;
            ram.cl = true;
            ram.proceed();
            Assert.AreEqual(0, Tools.boolArray2Int(ram.output));

            ram.ad = Tools.int2BoolArray(1);
            ram.X = Tools.int2BoolArray(33);
            ram.st = true;
            ram.cl = false;
            ram.proceed();
            Assert.AreEqual(0, Tools.boolArray2Int(ram.output));

            ram.ad = Tools.int2BoolArray(1);
            ram.X = Tools.int2BoolArray(33);
            ram.st = true;
            ram.cl = true;
            ram.proceed();
            Assert.AreEqual(0, Tools.boolArray2Int(ram.output));

            ram.ad = Tools.int2BoolArray(1);
            ram.X = Tools.int2BoolArray(22);
            ram.st = false;
            ram.cl = false;
            ram.proceed();
            Assert.AreEqual(33, Tools.boolArray2Int(ram.output));

            ram.ad = Tools.int2BoolArray(0);
            ram.X = Tools.int2BoolArray(22);
            ram.st = false;
            ram.cl = true;
            ram.proceed();
            Assert.AreEqual(44, Tools.boolArray2Int(ram.output));


        }
    }
}
