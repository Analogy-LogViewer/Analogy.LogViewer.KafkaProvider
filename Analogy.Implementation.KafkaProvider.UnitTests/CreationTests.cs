using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Analogy.Interfaces.Factories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Analogy.Implementation.KafkaProvider.UnitTests
{
    [TestClass]
    public class CreationTests
    {



        [TestMethod]
        public void CreationTest()
        {

            var factories = GetFactories();
            Assert.IsTrue(factories != null);
        }

        private List<IAnalogyFactory> GetFactories()
        {
            List<IAnalogyFactory> factories = new List<IAnalogyFactory>();
            try
            {
                Assembly assembly = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "Analogy.Implementation.KafkaProvider.dll"));
                Type[] types = assembly.GetTypes();
                foreach (Type aType in types)
                {
                    try
                    {
                        if (aType.GetInterface(nameof(IAnalogyFactory)) != null)
                        {
                            if (!(Activator.CreateInstance(aType) is IAnalogyFactory factory)) continue;
                            factories.Add(factory);
                            foreach (var provider in factory.DataProviders.Items)
                            {
                                provider.InitDataProvider();
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        Assert.Fail("Failed with error: " + e);
                    }

                }

                return factories;
            }
            catch (Exception e)
            {
                Assert.Fail("Failed with error: " + e);
                return new List<IAnalogyFactory>(0);
            }
        }

        public void TestStartConsuming()
        {

        }
    }
}
