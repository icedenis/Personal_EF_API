using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personal_EF_API.Interfaces
{


    // This is the Base Repository to connect so i can use it in every class other DB connvected 
    public interface IRepositoryBase<T> where T : class
    {
        Task<IList<T>> FindAll();
        Task<T> FindById(int id);
        Task<bool> isExist(int id);

        Task<bool> Create(T entity);

        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Save();


    }
}
