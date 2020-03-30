using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using System;
using System.Collections.Generic;

namespace Analogy.Implementation.KafkaProvider
{
    public class AnalogyKafkaDataProviderFactory : IAnalogyDataProvidersFactory
    {
        public string Title { get; } = "Analogy Kafka Providers";
        public IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider> { new AnalogyKafkaDataProvider() };

        public Guid FactoryId => AnalogyKafkaFactory.Id;

    }
}
