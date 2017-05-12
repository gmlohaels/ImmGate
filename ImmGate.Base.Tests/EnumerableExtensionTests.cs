using System;
using System.Collections.Generic;
using System.Linq;
using ImmGate.Base.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImmGate.Base.Tests
{
    [TestClass]
    [TestCategory("ImmGate-Extensions")]
    public class EnumerableExtensionTests
    {

        [TestMethod]
        public void SymmetricExceptWithJaggedSequencesShouldReturnElement()
        {
            var x = new List<int> { 1, 2 };
            var y = new List<int> { 1, 2, 5, 4 };
            var i = x.SymmetricExcept(y).First();
            Assert.IsTrue(i == 5);
        }
    }
}
