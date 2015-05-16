using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebDrive.DAL.Context;
using WebDrive.DAL.UnitOfWork;

namespace WebDrive.DAL.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public IUnitOfWork _UnitOfWork { get; set; }
        private WebContext Context { get; set; }
        private DbSet<TEntity> dbSet { get; set; }

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            this._UnitOfWork = unitOfWork;
            this.Context = unitOfWork.Context;
            this.dbSet = this.Context.Set<TEntity>();
        }

        public void Insert(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            this.dbSet.Add(instance);
        }

        public void Update(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            this.Context.Entry(instance).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            if (entityToDelete == null)
            {
                throw new ArgumentNullException("entityToDelete");
            }
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public IQueryable<TEntity> GetAll()
        {
            return dbSet;
        }

        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }

        }

        public TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {
            return dbSet.SqlQuery(query, parameters).ToList();
        }
    }
}