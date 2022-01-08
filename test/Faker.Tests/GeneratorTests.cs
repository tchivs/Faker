using Microsoft.VisualStudio.TestTools.UnitTesting;
using Faker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faker.Provider.CHS;

namespace Faker.Core.Tests
{
    [TestClass()]
    public class GeneratorTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        [DataRow(1000000)]
        public void SelectByWeightTest(int count)
        {
            var generator = new Generator(new GeneratorOptions());
            var lastNames = PersonProvider._lastNames;
            var weight = generator.GetWeightIndex(lastNames.Values);

            for (int i = 0; i < count; i++)
            {
            var name=    lastNames.Keys[ generator.GetRandomIndexByWidths(weight)];
            }
        }
    }
}