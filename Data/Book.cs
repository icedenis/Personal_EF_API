using System.ComponentModel.DataAnnotations.Schema;

namespace Personal_EF_API.Data
{
    //this are the same name as the colm and name in the Database so  need to make Automappper and Datatransever objects so i dont break the code
 [Table("Books")]
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string Isbn { get; set; }

        public string Summary { get; set; }
        public string Image { get; set; }
        public double? Price { get; set; }
        public int? AuthorId { get; set; }
        public virtual Author Author { get; set; }
      
    }
}