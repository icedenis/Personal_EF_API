using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//Data Transfer Objects Shared
namespace Personal_EF_API.Data.Mappings.DTOs
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Bio { get; set; }

        // Here is the exple that Autho  to book is 1:N rationship  1 autor many books in db
        public virtual IList<BookDTO> Books { get; set; }
    }
}
