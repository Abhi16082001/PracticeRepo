using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Chatting_Calling.Models
{
    [Table("users")]
    public class ApplicationUser
    {
        public string Id { get; set; }   // User ID (GUID or Identity)
        public string UserName { get; set; }
        public string PasswordHash { get; set; }  // In real apps, use ASP.NET Identity
    }
}