using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Faker.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Faker.Tests.Providers
{
    [TestClass]
    public abstract class ProviderTest
    {
        public FakerBuilder GetFaker(string name)
        {
            return new FakerBuilder()
                .UseCultureInfo(name);

        }
    }
    [TestClass]
    public class InternetProviderTest : ProviderTest
    {
        [TestMethod]
        [DataRow("zh-CN", 100)]
        [DataRow("zh-CN", 1000000)]
        public void TestPortNumber(string name, int count)
        {
            using var faker = GetFaker(name).Build();
            for (int i = 0; i < count; i++)
            {
                var port = faker.Internet.PortNumber();
                Assert.IsTrue(port < 65535);
            }
        }

        [TestMethod]
        [DataRow("zh-CN", 100)]
        public void TestMacAddress(string name, int count)
        {
            using var faker = GetFaker(name)
                .Configure(opt => opt.Provider.Internet.MacAddress.Separator = "-").Build();
            for (int i = 0; i < count; i++)
            {
                var mac = faker.Internet.MacAddress();
                Console.WriteLine(mac);
            }
        }
    }
    [TestClass]
    public class PersonProviderTest : ProviderTest
    {
        [TestMethod]
        //[DataRow("en-US", 100)]
        [DataRow("zh-CN", 100)]
        [DataRow("zh-CN", 100, true)]
        [DataRow("zh-CN", 1000000)]
        [DataRow("zh-CN", 1000000, true)]
        public void TestGetName(string name, int count, bool useWeighting = false)
        {
            using var faker = GetFaker(name).Configure(opt => opt.Provider.UseWeighting = useWeighting).Build();
            Assert.IsNotNull(faker.Person);
            var n = faker.Person["Name"];
            Console.WriteLine(n);
            for (int i = 0; i < count; i++)
            {
                faker.Person.Name();
            }
        }

        [TestMethod]
        [DataRow("zh-CN", 100)]
        [DataRow("zh-CN", 1000000)]
        public void TestGetName_Indexer(string name, int count)
        {
            var faker = GetFaker(name).Build();
            Assert.IsNotNull(faker.Person);
            for (int i = 0; i < count; i++)
            {
                var n = faker.Person["Name"];
            }
        }
        [TestMethod]
        [DataRow("zh-CN", 100)]
        [DataRow("zh-CN", 1000000)]
        public void TestGetNameMale(string name, int count)
        {
            var faker = GetFaker(name).Build();
            Assert.IsNotNull(faker.Person);
            for (int i = 0; i < count; i++)
            {
                faker.Person.NameMale();
            }
        }
        [TestMethod]
        [DataRow("zh-CN", 100)]
        public void TestGetRomanizedName(string name, int count)
        {
            var faker = GetFaker(name)
                .Configure(opt => opt.Provider.Person.RomanizedWithSpace = false)
                .Build();
            Assert.IsNotNull(faker.Person);
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(faker.Person.RomanizedName());
            }
        }
        [TestMethod]
        [DataRow("zh-CN", 100)]
        [DataRow("zh-CN", 1000000)]
        public void TestGetNameFemale(string name, int count)
        {
            var faker = GetFaker(name).Build();
            Assert.IsNotNull(faker.Person);
            for (int i = 0; i < count; i++)
            {
                faker.Person.NameFemale();
            }
        }
    }
}
