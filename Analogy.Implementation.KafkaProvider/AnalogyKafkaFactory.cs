using System;
using System.Collections.Generic;
using System.Drawing;
using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.LogViewer.KafkaProvider.Properties;
using Analogy.LogViewer.Template;

namespace Analogy.LogViewer.KafkaProvider
{
    public class AnalogyKafkaFactory : PrimaryFactory
    {
        internal static Guid Id = new Guid("FC2115F6-058A-430B-8E41-385E7A3DF3A9");
        public override Guid FactoryId { get; set; } = Id;
        public override string Title { get; set; } = "Analogy Kafka Provider";
        public override Image SmallImage { get; set; } = Resources.Analogy_image_16x16;
        public override Image LargeImage { get; set; } = Resources.Analogy_image_32x32;
        public override IEnumerable<string> Contributors { get; set; } = new List<string>() { "Lior Banai" };
        public override string About { get; set; } = "Kafka Provider for Analogy";

        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = new List<AnalogyChangeLog>
        {
            new AnalogyChangeLog("Add SourceLink",AnalogChangeLogType.Improvement, "Lior Banai",new DateTime(2020, 02, 10)),
            new AnalogyChangeLog("Add multi topic subscription",AnalogChangeLogType.None, "Lior Banai",new DateTime(2019, 10, 31)),
            new AnalogyChangeLog("Create Initial implementation",AnalogChangeLogType.None, "Lior Banai",new DateTime(2019, 10, 19))
        };
    }
}
