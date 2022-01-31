using System;
using System.Collections.Generic;
using System.Reflection;

namespace DalApi
{
    /// <summary>
    /// give an object of class that implement the interface IDal according to the choice in the file dal-config.
    /// </summary>
    public static class DalFactory
    {
        public static IDal GetDal()
        {
            string dlType;
            dlType = DalConfig.DalName; 
            DalConfig.DalPackage dalPackage;
            try // get dal package info according to dal element value in config file
            {
                dalPackage = DalConfig.DalPackages[dlType];
            }
            catch (KeyNotFoundException ex)
            {
                // if package name is not found in the list 
                throw new DalConfigException($"Wrong DL type: {dlType}", ex);
            }
            string dlPackageName = dalPackage.PkgName;
            string dlNameSpace = dalPackage.NameSpace;
            string dlClass = dalPackage.ClassName;

            try // Load the dal implementation assembly according to dll file name
            {
                Assembly.Load(dlPackageName);
            }
            catch (Exception ex) when (ex is System.IO.FileLoadException || ex is System.IO.FileNotFoundException)
            { 
                throw new DalConfigException($"Failed loading {dlPackageName}.dll", ex);
            }
            Type type;
            try
            {
                type = Type.GetType($"{dlNameSpace}.{dlClass}, {dlPackageName}", true);
            }
            catch (TypeLoadException ex)
            { // If the type is not found 
                throw new DalConfigException($"Class not found due to a wrong namespace or/and class name: {dlPackageName}:{dlNameSpace}.{dlClass}", ex);
            }
            try
            {
                IDal dal = type.GetProperty("Instance", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as IDal;
                // If the instance property is not initialized
                if (dal == null)
                    throw new DalConfigException($"Class {dlNameSpace}.{dlClass} instance is not initialized");
                return dal;
            }
            catch (NullReferenceException ex)
            {
                throw new DalConfigException($"Class {dlNameSpace}.{dlClass} is not a singleton", ex);
            }
        }
    }
}
