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
        /// constructor. call the static function initialize().
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
            double lat1 = lattitudeA * (Math.PI / 180.0);
            double long1 = longitudeA * (Math.PI / 180.0);
            double lat2 = lattitudeB * (Math.PI / 180.0);
            double long2 = longitudeB * (Math.PI / 180.0) - long1;
            double distance = Math.Pow(Math.Sin((lat2 - lat1) / 2.0), 2.0) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(long2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(distance), Math.Sqrt(1.0 - distance))) / 1000;
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