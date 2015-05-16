using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebDrive.DAL.Context;

namespace WebDrive.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        WebContext Context { get; }

        int SaveChange();
    }
}