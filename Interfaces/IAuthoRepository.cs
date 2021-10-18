using Personal_EF_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personal_EF_API.Interfaces
{
    // here i extende the autho repository base with main functions in the author inteface 
    public interface IAuthoRepository : IRepositoryBase<Author>
    {

    }
}
