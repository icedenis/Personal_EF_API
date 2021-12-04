using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Personal_EF_API.Data;

namespace Personal_EF_API.Interfaces
{
  public  interface  IBookRepository : IRepositoryBase<Book>
    {
        //need for the update the file if the there is changes in the file name in the Uploads folder
        public Task<string> GetImageFileName(int id);
    }
}
