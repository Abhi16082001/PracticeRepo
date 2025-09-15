using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace Chatting_Calling.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext() : base("ConString") { }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

    }
}