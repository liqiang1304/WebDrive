using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDrive.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebDrive.DAL.Context
{
    public class WebContext : DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<QRCode> QRCodes { get; set; }
        public DbSet<ValidationCode> ValidationCodes { get; set; }
        public DbSet<RealFile> RealFiles { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<Recoder> Recoders { get; set; }

        public WebContext()
        {
        }

        public WebContext(string connectionString):base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}