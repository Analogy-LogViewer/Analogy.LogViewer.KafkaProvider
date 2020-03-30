using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Analogy.Implementation.KafkaProvider.UnitTests
{
    [TestClass]
    public class CreationTests
    {



        [TestMethod]
        public async Task CreationTest()
        {

            //var factories = GetFactories();
            //Assert.IsTrue(factories != null);

            //foreach (var d in factories.Last().DataProviders.Items)
            //{
            //    await d.InitializeDataProviderAsync(null);
            //    (d as IAnalogyRealTimeDataProvider)?.StartReceiving();
            //}

            //await Task.Delay(1000);
        }

        //private List<IAnalogyFactory> GetFactories()
        //{
        //    List<IAnalogyFactory> factories = new List<IAnalogyFactory>();
        //    try
        //    {
        //        Assembly assembly = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "Analogy.LogViewer.KafkaProvider.dll"));
        //        Type[] types = assembly.GetTypes();
        //        foreach (Type aType in types)
        //        {
        //            try
        //            {
        //                if (aType.GetInterface(nameof(IAnalogyFactory)) != null)
        //                {
        //                    if (!(Activator.CreateInstance(aType) is IAnalogyFactory factory)) continue;
        //                    factories.Add(factory);
        //                    foreach (var provider in factory.DataProviders.Items)
        //                    {
        //                        provider.InitializeDataProviderAsync(null);
        //                    }

        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                Assert.Fail("Failed with error: " + e);
        //            }

        //        }

        //        return factories;
        //    }
        //    catch (Exception e)
        //    {
        //        Assert.Fail("Failed with error: " + e);
        //        return new List<IAnalogyFactory>(0);
        //    }
        //}

        public void TestStartConsuming()
        {

        }
    }
}
