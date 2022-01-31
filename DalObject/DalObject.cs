using DO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;


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
            //foreach (Drone drone in DataSource.drones)
            //{
            //    Random rand = new Random();
            //    int goToMaintanence = rand.Next(2);
            //    if (goToMaintanence == 1)
            //    {
            //        int index = rand.Next(0, DataSource.stations.Count());
            //        Station stationForFirstCharge = DataSource.stations.ElementAt(index);
            //        SendDroneToCharge(drone.Id, stationForFirstCharge.Id);
            //    }
            //}
        }
        /// <summary>
        /// The public Instance property to use
        /// </summary>
        internal static DalObject Instance
        {
            //singelton thread safe and lazy initializion
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

        /// <summary>
        /// return strong password
        /// </summary>
        /// <returns></returns>
        internal static string getGoodPass()
        {
            Random rand = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[8];
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[rand.Next(chars.Length)];
            }
            return new String(stringChars);
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetManagmentPassword()
        {
            return DataSource.Config.managmentPassword;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string SetNewManagmentPassword()
        {
            string pass = getGoodPass();
            DataSource.Config.managmentPassword = pass;
            return pass;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public User GetUser(string name)
        {
            User? result = DataSource.users.Find(x => x.UserName == name);
            if (result == null)
                throw new UserException($"UserName {name} does not exist");
            return (User)result;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddUser(User user)
        {
            if (user.IsManager == false && DataSource.users.Exists(item => item.Id == user.Id))
                throw new UserException($"User for the customer {user.Id} already exist");
            if (DataSource.users.Exists(item => item.UserName == user.UserName))
                throw new UserException($"User with the usernam '{user.Id}' already exist");
            DataSource.users.Add(user);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetUsersList(Predicate<User> pre)
        {
            List<User> result = new List<User>(DataSource.users);
            if (pre == null) return result;
            return result.FindAll(pre);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] GetBatteryData()
        {
            return new double[] {
                DataSource.Config.available,
                DataSource.Config.easy,
                DataSource.Config.medium,
                DataSource.Config.heavy,
                DataSource.Config.ratePerMinute };
        }
    }
}