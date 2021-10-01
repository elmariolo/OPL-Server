using System;
using System.Collections.Generic;

namespace OPLServer
{
    public class UserCollection : List<User>
    {
        public void Add(string accountName, string password)
        {
            Add(new User(accountName, password));
        }

        public int IndexOf(string accountName)
        {
            for (int index = 0; index < this.Count; index++)
            {
                if (string.Equals(this[index].AccountName, accountName, StringComparison.OrdinalIgnoreCase))
                {
                    return index;
                }
            }
            return -1;
        }

        public string GetUserPassword(string accountName)
        {
            int index = IndexOf(accountName);
            if (index >= 0)
            {
                return this[index].Password;
            }
            return null;
        }

        public List<string> ListUsers()
        {
            List<string> result = new List<string>();
            foreach (User user in this)
            {
                result.Add(user.AccountName);
            }
            return result;
        }
    }

    public class User
    {
        public string AccountName;
        public string Password;

        public User(string accountName, string password)
        {
            AccountName = accountName;
            Password = password;
        }
    }
}
