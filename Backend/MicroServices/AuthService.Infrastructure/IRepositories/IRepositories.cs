using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.IRepositories
{
    public interface IRepositories<T> where T : class, new()
    {
        List<T> GetAll();
        T Get(int id);
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(int id);

    }
}
