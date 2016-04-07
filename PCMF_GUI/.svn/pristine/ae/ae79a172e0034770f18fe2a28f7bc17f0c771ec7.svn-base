using System.Collections.Generic;

namespace IPS.Navigation
{
    public class ModuleMapper
    {
            public static Dictionary<string, string> ModuleMaps { get; set; }

            static ModuleMapper()
            {
                // if any navigation pages have prism regions then put the map to the relevant
                // module here.  The module will then be dynamically loaded when necessary.
                ModuleMaps = new Dictionary<string, string>();
                ModuleMaps.Add("/DataCollectionModule", "IPS.DataCollection");
                ModuleMaps.Add("/ModelCreationModule", "IPS.ModelCreation");
                ModuleMaps.Add("/ConfigurationModule", "IPS.Configuration");
                ModuleMaps.Add("/HistoricalDataModule", "IPS.HistoricalData");
            }
        }
}
