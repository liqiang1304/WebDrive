using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDrive.DAL.Context;

namespace WebDrive.DAL.DbContextFactory
{
    public interface IDbContextFactory
    {
        WebContext GetDbContext();
    }
}
