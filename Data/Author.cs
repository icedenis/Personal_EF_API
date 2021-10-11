using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_EF_API.Data
{
    //Here is the Data allocations
    [Table("Authors")]
    public partial class Author
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Bio { get; set; }

        // Here is the exple that Autho  to book is 1:N rationship  1 autor many books in db
        public virtual IList<Book> Books { get; set; }

    }
}