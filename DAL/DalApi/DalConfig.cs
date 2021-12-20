using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DalApi
{
    //static class DalConfig
    //{
    //    public class DalPackage
    //    {
    //        public string Name;
    //        public string PkgName;
    //        public string NameSpace;
    //        public string ClassName;
    //    }

    //    internal static string DalName;
    //    internal static Dictionary<string, DalPackage> DalPackages;
    //    static DalConfig()
    //    {
    //        XElement dalConfig = XElement.Load(@"dal-config.xml");
    //        DalName = dalConfig.Element("dal").Value; //what is?
    //        DalPackages = (from package in dalConfig.Element("dal-packages").Elements()
    //                       let temp1 = package.Attribute("namespace")
    //                       let nameSpace = temp1 == null ? "dal" : temp1.Value
    //                       let temp2 = package.Attribute("class")
    //                       let className = temp2 == null ? package.Value : temp2.Value
    //                       select new DalPackage()
    //                       {
    //                           Name = "" + package.Name,
    //                           PkgName = package.Value,
    //                           NameSpace = nameSpace,
    //                           ClassName = className
    //                       })
    //                     .ToDictionary(p => "" + p.Name, p => p);
    //    }
    //}
    class DalConfig
    {
        internal static string DalName;
        internal static Dictionary<string, string> DalPackages;
        static DalConfig()
        {
            XElement dalConfig = XElement.Load(@"xml\dal-config.xml");
            DalName = dalConfig.Element("dal").Value;
            DalPackages = (from pkg in dalConfig.Element("dal-packages").Elements()
                           select pkg
                           ).ToDictionary(p => "" + p.Name, p => p.Value);
        }
    }
    public class DalConfigException : Exception
    {
        public DalConfigException(string msg) : base(msg) { }
        public DalConfigException(string msg, Exception ex) : base(msg, ex) { }
    }
}
