﻿using System;
using System.Collections.Generic;
using System.Drawing;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.LogViewer.KafkaProvider.Properties;

namespace Analogy.LogViewer.KafkaProvider.Example
{
    public class AnalogyKafkaExampleFactory : Template.PrimaryFactory
    {
        internal static Guid Id = new Guid("CFE5834B-806A-4DB0-B36C-7E2C67DE2ECF");

        public override Guid FactoryId { get; set; } = Id;
        public override string Title { get; set; } = "Analogy Kafka Example";
        public override Image SmallImage { get; set; } = Resources.Analogy_image_16x16;
        public override Image LargeImage { get; set; } = Resources.Analogy_image_32x32;
        public override IEnumerable<string> Contributors { get; set; } = new List<string>() { "Lior Banai" };
        public override string About { get; set; } = "Kafka Provider for Analogy (Producer example)";

        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = new List<AnalogyChangeLog>
        {
            new AnalogyChangeLog("Create Initial implementation (example)",AnalogChangeLogType.None, "Lior Banai",new DateTime(2019, 10, 20), "")
        };

        public class AnalogyKafkaExampleDataProviderFactory : IAnalogyDataProvidersFactory
        {
            public string Title { get; set; } = "Analogy Kafka Providers Example";
            public IEnumerable<IAnalogyDataProvider> DataProviders { get; set; } = new List<IAnalogyDataProvider> { new AnalogyKafkaExampleDataProvider() };

            public Guid FactoryId { get; set; } = AnalogyKafkaExampleFactory.Id;

        }
    }
}
