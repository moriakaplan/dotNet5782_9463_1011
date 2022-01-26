using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BLApi;
using DO;
using System.Runtime.CompilerServices;

namespace BL
{
    internal partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetManagmentPassword()
        {
            lock (dl)
            {
                return dl.GetManagmentPassword();
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string ChangeManagmentPassword()
        {
            lock (dl)
            {
                return dl.SetNewManagmentPassword();
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetUserId(string name, string password)
        {
            try
            {
                User user;
                lock (dl)
                {
                    user = dl.GetUser(name);
                }
                if (user.Password != password) throw new NotExistIDException("the password is not correct");
                if (user.IsManager) throw new NotExistIDException("the user is manager and not a regular user");
                return (int)user.Id;
            }
            catch (UserException)
            {
                throw new NotExistIDException("not exist user with this user name");
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool ExistManager(string name, string password)
        {
            try
            {
                User user;
                lock (dl)
                {
                    user = dl.GetUser(name);
                }
                if (user.Password != password) return false;
                if (user.IsManager == false) return false;
                return true;
            }
            catch (UserException)
            {
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddUser(int id, string name, string password)
        {

            DO.User user = new DO.User
            {
                Id = id,
                UserName = name,
                Password = password,
                IsManager = false
            };
            try
            {
                lock (dl)
                {
                    dl.AddUser(user); //add the new customer to the list in the data level
                }
            }
            catch (DO.UserException)
            {
                throw new ExistIdException("this id or username already exist");
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddManager(string name, string password)
        {
            DO.User user = new DO.User
            {
                Id = null,
                UserName = name,
                Password = password,
                IsManager = true
            };
            try
            {
                lock (dl)
                {
                    dl.AddUser(user); //add the new customer to the list in the data level
                }
            }
            catch (DO.UserException)
            {
                throw new ExistIdException("this username already exist");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<string> GetListOfManagersNames()
        {
            lock (dl)
            {
                return dl.GetUsersList(x => x.IsManager == true).Select(x => x.UserName);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<string> GetListOfUsersNames()
        {
            lock (dl)
            {
                return dl.GetUsersList(x => x.IsManager == false).Select(x => x.UserName);
            }
        }
    }
}
