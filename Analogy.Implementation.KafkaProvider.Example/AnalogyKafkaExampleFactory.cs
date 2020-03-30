using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;

namespace Analogy.Implementation.KafkaProvider.Example
{
    public class AnalogyKafkaExampleFactory : IAnalogyFactory
    {
        internal static Guid Id = new Guid("CFE5834B-806A-4DB0-B36C-7E2C67DE2ECF");
        public Guid FactoryId { get; } = Id;
        public string Title { get; } = "Analogy Kafka Example";
        public IEnumerable<string> Contributors { get; } = new List<string>() { "Lior Banai" };
        public string About { get; } = "Kafka Provider for Analogy (Producer example)";

        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; } = new List<AnalogyChangeLog>
        {
            new AnalogyChangeLog("Create Initial implementation (example)",AnalogChangeLogType.None, "Lior Banai",new DateTime(2019, 10, 20))
        };

        public class AnalogyKafkaExampleDataProviderFactory : IAnalogyDataProvidersFactory
        {
            public string Title { get; } = "Analogy Kafka Providers Example";
            public IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider> { new AnalogyKafkaExampleDataProvider() };

            public Guid FactoryId => AnalogyKafkaExampleFactory.Id;

        }
    }
}
