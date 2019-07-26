using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RandomSquadCreater
{
    public class Repository<T> : IRepository<T> where T : class
    {
        
        private readonly DbContext _dbContext; 
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentException("DbContext nesnesi null olamaz!!");

            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();

        }
      
        public IList<T> GetAll()
        {
            return _dbSet.ToList();
        }
        public IList<T> GetAll(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, params Expression<Func<T, object>>[] includes)//default null
        {
            IQueryable<T> sorgu = _dbSet;
            if (predicate != null)
                sorgu = sorgu.Where(predicate);
            if (orderby != null)
                sorgu = orderby(sorgu); 
            foreach (Expression<Func<T, object>> tablo in includes)
            {
                sorgu = sorgu.Include(tablo);

            }


            return sorgu.ToList();

        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
 
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
   
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            else
            {
                //silmeyip silindi true yapıcaksak if yazılır
                if (entity.GetType().GetProperty("Silindi") != null)//böyle kolon varsa
                {
                    T _entity = entity;
                    _entity.GetType().GetProperty("Silindi").SetValue(_entity, true);
                    this.Update(_entity);
                }
                else
                {
                    Delete(entity);
                }

            }
            



        }

        public void Delete(T entity)
        {

            
            if (entity.GetType().GetProperty("Silindi") != null)
            {
                T _entity = entity;
                _entity.GetType().GetProperty("Silindi").SetValue(_entity, true);
                this.Update(_entity);
            }
            else
            {
                // Delete(entity);
                //öncelikle entity nin state özelliğini kontrol etmeliyiz
                DbEntityEntry dbentry = _dbContext.Entry(entity);
                if (dbentry.State != EntityState.Deleted)
                {
                    dbentry.State = EntityState.Deleted;

                }
                else
                {
                    _dbSet.Attach(entity);
                    _dbSet.Remove(entity);
                }
                


            }


            
        }

        public T Get(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, params Expression<Func<T, object>>[] includes)//default null
        {
            IQueryable<T> sorgu = _dbSet;
            if (predicate != null)
                sorgu = sorgu.Where(predicate);
            if (orderby != null)
                sorgu = orderby(sorgu); 
            foreach (Expression<Func<T, object>> tablo in includes)
            {
                sorgu = sorgu.Include(tablo);

            }


            return sorgu.FirstOrDefault();

        }




    }
}
