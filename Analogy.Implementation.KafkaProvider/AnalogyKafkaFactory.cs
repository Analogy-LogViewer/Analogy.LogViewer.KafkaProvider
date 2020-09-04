using System;
using System.Collections.Generic;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;

namespace Analogy.LogViewer.KafkaProvider
{
    public class AnalogyKafkaFactory : IAnalogyFactory
    {
        internal static Guid Id = new Guid("FC2115F6-058A-430B-8E41-385E7A3DF3A9");
        public Guid FactoryId { get; set; } = Id;
        public string Title { get; set; } = "Analogy Kafka Provider";
        public IAnalogyDataProvidersFactory DataProviders { get; } = new AnalogyKafkaDataProviderFactory();
        public IAnalogyCustomActionsFactory Actions { get; } = null;
        public IEnumerable<string> Contributors { get; set; } = new List<string>() { "Lior Banai" };
        public string About { get; set; } = "Kafka Provider for Analogy";

        public IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = new List<AnalogyChangeLog>
        {
            new AnalogyChangeLog("Add SourceLink",AnalogChangeLogType.Improvement, "Lior Banai",new DateTime(2020, 02, 10)),
            new AnalogyChangeLog("Add multi topic subscription",AnalogChangeLogType.None, "Lior Banai",new DateTime(2019, 10, 31)),
            new AnalogyChangeLog("Create Initial implementation",AnalogChangeLogType.None, "Lior Banai",new DateTime(2019, 10, 19))
        };
    }
}
