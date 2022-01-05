using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BLApi;
using DO;

namespace BL
{
    internal partial class BL
    {
        public int GetUserId(string name, string password)
        {
            try
            {
                User user =dl.GetUser(name);
                if (user.Password != password) throw new NotExistIDException("the password is not correct");
                if (user.IsManager) throw new NotExistIDException("the user is manager and not a regular user");
                return (int)user.Id;
            }
            catch(UserException)
            {
                throw new NotExistIDException("not exist user with this user name");
            }
        }
        public bool ExistManager(string name, string password)
        {
            try
            {
                User user = dl.GetUser(name);
                if (user.Password != password) return false; //throw new NotExistIDException("the password is not correct");
                if (user.IsManager==false) return false; //throw new NotExistIDException("the user is a regular user and not a manager");
                return true;
            }
            catch (UserException)
            {
                //throw new NotExistIDException("not exist user with this user name");
                return false;
            }
        }

        public void AddUser(int id, string name, string password)
        {
            //צריך לבדוק שאין כבר משתמש לאידי הזה ושהשם משתמש לא תפוס
        }
    }
}
