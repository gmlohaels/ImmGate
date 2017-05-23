using System.Collections.Generic;
using ImmGate.Base.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImmGate.Base.Tests.Extensions
{
    [TestClass]
    public class EnumRandomTests
    {

        public enum TestEnum
        {
            Invalid,
            One,
            Two,
            Three,
            Four,
        }

        [TestMethod]
        public void EnsureOnlyAllowedRandomEnumValueBeGenerated()
        {
            var te = TestEnum.Invalid;
            Assert.IsTrue(te.RandomValue(new List<string>() { "One" }) == TestEnum.One);
        }


        [TestMethod]
        public void EnsureSpecificValueWillBeGeneratedUsingSeed()
        {
            var te = TestEnum.Invalid;
            var valueOneSeed = 3;
            Assert.IsTrue(te.RandomValue(new List<string>() { "One", "Two", "Three" }, valueOneSeed) == TestEnum.One);
        }


    }
}
