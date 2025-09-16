using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chatting_Calling.Models
{
    public class Services
    {
        public static List<string> Getallusers()
        {
            List<string> users;
            using (var db= new ApplicationDbContext())
            {
                users = db.Users.Select(x=>x.Id).ToList();
            }
                return users;

        }

    }
}