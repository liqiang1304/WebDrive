using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebDrive.DAL.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Insert(TEntity instance);

        void Update(TEntity instance);

        void Delete(TEntity instance);

        void Delete(object id);

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties);

        TEntity GetByID(object id);

        IQueryable<TEntity> GetAll();

        IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters);
    }
}
