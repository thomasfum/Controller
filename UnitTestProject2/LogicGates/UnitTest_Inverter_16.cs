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
    public class UnitTest_Inverter_16
    {

        
        [TestMethod]
        public void TestMethod_2()
        {
            Inverter_16 inv = new Inverter_16();
            inv.a = Tools.int2BoolArray(2);
            int val=inv.proceed();
            Assert.AreEqual(-3, Tools.boolArray2Int(inv.s));
            Assert.AreEqual(48, val);
        }
    }
}
