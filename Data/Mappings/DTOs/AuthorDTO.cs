using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

 
    public class CreatAuthorDTO
    {
        [Required]
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string BIO { get; set; }
    }

    public class UpdateAuthorDTO
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string BIO { get; set; }
    }
}
