using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GST_Designs.Models
{
    public static class Repository
    {
        static List<User> users = new List<User>()
        {
            new User() {Email = "Thimiragst@gmail.com",Roles = "Admin", Password="Thimiragst12345" },
            new User() {Email="xyz@gmail.com",Roles="Editor",Password="xyzeditor"}
        };

        public static User GetUserDetails(User user)
        {
            return users.Where(u => u.Email.ToLower().Equals(user.Email.ToLower()) &&
                                    u.Password.Equals(user.Password)).FirstOrDefault();
        }
    }
}