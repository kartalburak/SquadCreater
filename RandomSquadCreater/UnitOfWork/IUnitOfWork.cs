using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomSquadCreater
{
    public interface IUnitOfWork : IDisposable
    {

        IRepository<T> Repository<T>() where T : class;
        int SaveChanges();

    }
}
