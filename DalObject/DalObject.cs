using DO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal partial class DalObject : IDal
    {
        private static DalObject instance;
        private static object syncRoot = new object();
        /// <summary>
        /// constructor.call the static function initialize.
        /// </summary>
        private DalObject()
        {
            DataSource.Initialize();
        }
        /// <summary>
        /// The public Instance property to use
        /// </summary>
        internal static DalObject Instance
        {
            //singelton thread safe and lazy initializion(?)
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DalObject();
                    }
                }
                return instance;
            }
        }

        internal string getGoodPass()
        {
            Random rand = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[8];
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[rand.Next(chars.Length)];
            }
            return new String(stringChars);
        }

        public string getManagmentPassword()
        {
            return DataSource.Config.managmentPassword;
        }

        public string setNewManagmentPassword()
        {
            string pass = getGoodPass();
            DataSource.Config.managmentPassword = pass;
            return pass;
        }

        public User GetUser(string name)
        {
            User? result = DataSource.users.Find(x => x.UserName == name);
            if (result == null)
                throw new UserException($"UserName {name} does not exist");
            return (User)result;
        }

        public void AddUser(User user)
        {
            if (user.IsManager == false && DataSource.users.Exists(item => item.Id == user.Id))
                throw new UserException($"User for the customer {user.Id} already exist");
            if (DataSource.users.Exists(item => item.UserName == user.UserName))
                throw new UserException($"User with the usernam '{user.Id}' already exist");
            DataSource.users.Add(user);
        }

        public double Distance(double lattitudeA, double longitudeA, double lattitudeB, double longitudeB)
        {
            var radiansOverDegrees = (Math.PI / 180.0);

            var latitudeRadiansA = lattitudeA * radiansOverDegrees;
            var longitudeRadiansA = longitudeA * radiansOverDegrees;
            var latitudeRadiansB = lattitudeB * radiansOverDegrees;
            var longitudeRadiansB = longitudeB * radiansOverDegrees;
            // Haversine formula
            double dlon = longitudeB - longitudeA;
            double dlat = lattitudeB - lattitudeA;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                       Math.Cos(lattitudeA) * Math.Cos(lattitudeB) *
                       Math.Pow(Math.Sin(dlon / 2), 2);
            double c = 2 * Math.Asin(Math.Sqrt(a));
            //Radius of earth in kilometers.
            double r = 6371;
            // calculate the result
            return (c * r);
        }
        public double[] GetBatteryData()
        {
            return new double[] {
                DataSource.Config.available,
                DataSource.Config.easy,
                DataSource.Config.medium,
                DataSource.Config.heavy,
                DataSource.Config.ratePerHour };
        }
    }
}