using BlogDAL.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericService<T> GetRepository<T>() where T : class;
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
