using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Personal_EF_API.Interfaces;
using Personal_EF_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Personal_EF_API.Services
{
    /// <summary>
    /// Thats the EF for Author Table Connects to DB then i have the the intefaces from the Base Repository 
    /// </summary>
    public class AuthorRepository : IAuthoRepository
    {

        //Here i need Object from the ApplicationDb context so i can read the data from the Dtabase

        private readonly ApplicationDbContext _db;

        public AuthorRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public  async Task<bool> Create(Author entity)
        {
            // here how i add in EF new Author
           await _db.Authors.AddAsync(entity);
            // here i also need to retunrn that i saved the data
            return await Save();
        }

        public async Task<bool> Delete(Author entity)
        {
            _db.Authors.Remove(entity);
            return await Save();
        }

        public async  Task<IList<Author>> FindAll()
        {
            // here i call the Applicaiton DB define Authors object table and ToLisasyn is from EF 
            var authors = await _db.Authors
               .Include(q => q.Books)
               .ToListAsync();

            return authors;
        }

        public async Task<Author> FindById(int id)
        {
            var author = await _db.Authors
           .Include(q => q.Books)
           .FirstOrDefaultAsync(q => q.Id == id);
            return author;
        }

        public async Task<bool> isExist(int id)
        {
            return await _db.Authors.AnyAsync(q=> q.Id==id);
           
        }

        public  async Task<bool> Save()
        {
            //here the rest is done with EF functions thats how i save 
            var changes =  await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Author entity)
        {
            _db.Authors.Update(entity);
            return await Save();
        }
    }
}
