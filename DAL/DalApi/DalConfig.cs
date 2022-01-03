using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DalApi
{
    /// <summary>
    /// find and save the option that choosed to be the implementation data layer.
    /// </summary>
    static class DalConfig
    {
        /// <summary>
        /// all the data of specific option
        /// </summary>
        public class DalPackage
        {
            public string Name;
            public string PkgName;
            public string NameSpace;
            public string ClassName;
        }

        internal static string DalName;
        internal static Dictionary<string, DalPackage> DalPackages;
        /// <summary>
        /// constractor, load the file dal-config and find wich option choosed
        /// </summary>
        static DalConfig()
        {
            XElement dalConfig = XElement.Load(@"xml/dal-config.xml");
            DalName = dalConfig.Element("dal").Value;
            DalPackages = (from package in dalConfig.Element("dal-packages").Elements()
                           let temp1 = package.Attribute("namespace")
                           let nameSpace = temp1 == null ? "dal" : temp1.Value
                           let temp2 = package.Attribute("class")
                           let className = temp2 == null ? package.Value : temp2.Value
                           select new DalPackage()
                           {
                               Name = "" + package.Name,
                               PkgName = package.Value,
                               NameSpace = nameSpace,
                               ClassName = className
                           })
                           .ToDictionary(p => "" + p.Name, p => p);
        }
    }
    
    [Serializable]
    public class DalConfigException : Exception
    {
        public DalConfigException(string message) : base(message) { }
        public DalConfigException(string message, Exception inner) : base(message, inner) { }
    }
}