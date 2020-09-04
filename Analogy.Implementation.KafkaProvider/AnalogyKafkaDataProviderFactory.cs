using System;
using System.Collections.Generic;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.KafkaProvider
{
    public class AnalogyKafkaDataProviderFactory : IAnalogyDataProvidersFactory
    {
        public string Title { get; set; } = "Analogy Kafka Providers";
        public IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider> { new AnalogyKafkaDataProvider() };

        public Guid FactoryId { get; set; } = AnalogyKafkaFactory.Id;

    }
}
