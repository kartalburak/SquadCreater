using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomSquadCreater
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbcontext;
        private bool disposed = false;

        public UnitOfWork(DbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            return new Repository<T>(_dbcontext);
        }

        public int SaveChanges()
        {
            try
            {
                return _dbcontext.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbcontext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }

}
