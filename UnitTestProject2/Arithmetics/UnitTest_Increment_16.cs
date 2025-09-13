using Controller;
using Controller.Arithmetics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject2.Arithmetics
{
    [TestClass]
    public class UnitTest_Increment_16
    {


        [TestMethod]
        public void TestMethod_2()
        {
            Increment_16 inc = new Increment_16();
            inc.a= Tools.int2BoolArray(2);
            int val=inc.proceed();
            Assert.AreEqual(3, Tools.boolArray2Int(inc.s));
            Assert.AreEqual(912, val);
        }
        [TestMethod]
        public void TestMethod_m2()
        {
            Increment_16 inc = new Increment_16();
            inc.a = Tools.int2BoolArray(-2);
            int val = inc.proceed();
            Assert.AreEqual(-1, Tools.boolArray2Int(inc.s));
            Assert.AreEqual(912, val);
        }
    }
}
