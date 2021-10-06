using System;
using System.Collections.Generic;

namespace OplServer.Interface
{
    public class UserCollection : List<User>
    {
        public void Add(string accountName, string password)
        {
            Add(new User(accountName, password));
        }

        public int IndexOf(string accountName)
        {
            for (var index = 0; index < Count; index++)
                if (string.Equals(this[index].AccountName, accountName, StringComparison.OrdinalIgnoreCase))
                    return index;
            return -1;
        }

        public string GetUserPassword(string accountName)
        {
            int index = IndexOf(accountName);
            if (index >= 0) return this[index].Password;
            return null;
        }

        public List<string> ListUsers()
        {
            var result = new List<string>();
            foreach (User user in this) result.Add(user.AccountName);
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