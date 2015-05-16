using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebDrive.DAL.Context;

namespace WebDrive.DAL.DbContextFactory
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly string _ConnectionString;

        public DbContextFactory(string connectionString)
        {
            this._ConnectionString = connectionString;
        }

        private WebContext _dbContext;

        private WebContext DbContext
        {
            get
            {
                if (this._dbContext == null)
                {
                    Type t = typeof(WebContext);
                    this._dbContext =
                        (WebContext)Activator.CreateInstance(t, this._ConnectionString);
                }
                return _dbContext;
            }
        }

        public WebContext GetDbContext()
        {
            return this.DbContext;
        }
    }
}