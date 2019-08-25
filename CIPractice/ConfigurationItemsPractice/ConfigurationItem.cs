using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationItemsPractice
{
    class ConfigurationItem
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public ConfigurationItem(string name, string version)
        {
            this.Name = name;
            this.Version = version;
        }
    }
}
