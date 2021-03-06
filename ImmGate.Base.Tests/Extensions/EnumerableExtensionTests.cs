﻿using System.Collections.Generic;
using System.Linq;
using ImmGate.Base.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImmGate.Base.Tests.Extensions
{
    [TestClass]
    [TestCategory("ImmGate-Extensions")]
    public class EnumerableExtensionTests
    {

        [TestMethod]
        [DataTestMethod]
        [DataRow(10, 3)]
        [DataRow(11, 4)]
        public void TakeRandomShouldAlwaysReturnSameForSpecificSeed(int seed, int value)
        {
            ImmgateRandom.SetSeed(seed);
            var r = new List<int>() { 1, 2, 3, 4 };
            int v = r.TakeRandom();
            Assert.IsTrue(v == value);

        }


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
