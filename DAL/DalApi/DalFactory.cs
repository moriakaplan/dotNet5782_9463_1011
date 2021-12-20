using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DalApi
{
    /// <summary>
    /// 
    /// </summary>
    static public class DalFactory
    {
        static public IDal GetDal()
        {
            //string type = DalConfig.DalName;
            //DalConfig.DalPackage dalPackage;
            //try
            //{
            //    dalPackage = DalConfig.DalPackages[type];
            //}
            //catch (KeyNotFoundException)
            //{
            //    throw new DalConfigException($"the type '{type}' is not correct");
            //}
            //try
            //{
            //    Assembly.Load(dalPackage.Name);
            //}
            //catch (Exception ex)
            //{
            //    throw new DalConfigException($"failed loading {dalPackage.Name}.dll", ex);
            //}
            //Type t = Type.GetType($"dal.{dalPackage.Name}, {dalPackage.Name}");
            //if (t == null) throw new DalConfigException($"class name is not the same as assembly name '{dalPackage.Name}'");
            //try
            //{
            //    IDal dal = t.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static) //we dont understand from here
            //        .GetValue(null) as IDal;
            //    // If the instance property is not initialized (i.e. it does not hold a real instance reference)...
            //    if (dal == null)
            //        throw new DalConfigException($"Class {dalPackage} instance is not initialized");
            //    // now it looks like we have appropriate dal implementation instance :-)
            //    return dal;
            //}
            //catch (NullReferenceException ex)
            //{
            //    throw new DalConfigException($"Class {dalPackage} is not a singleton", ex);
            //}

            string dalType = DalConfig.DalName;
            string dalPkg = DalConfig.DalPackages[dalType];
            if (dalPkg == null) throw new DalConfigException($"Package {dalType} is not found in packages list in dal-config.xml");

            try { Assembly.Load(dalPkg); }
            catch (Exception) { throw new DalConfigException("Failed to load the dal-config.xml file"); }

            Type type = Type.GetType($"Dal.{dalPkg}, {dalPkg}");
            if (type == null) throw new DalConfigException($"Class {dalPkg} was not found in the {dalPkg}.dll");

            IDal dal = (IDal)type.GetProperty("Instance",
                      BindingFlags.Public | BindingFlags.Static).GetValue(null);
            if (dal == null) throw new DalConfigException($"Class {dalPkg} is not a singleton or wrong propertry name for Instance");

            return dal;


        }
    }
}
