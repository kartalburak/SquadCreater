using System.Collections.Generic;

namespace RandomSquadCreater
{
    public interface IRepository<T> where T : class
    {

        IList<T> GetAll();

        // IList<T> GetAll(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, params Expression<Func<T, object>>[] includes);

        T GetById(int id);

        // T Get(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, params Expression<Func<T, object>>[] includes);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(int id);




    }
}
