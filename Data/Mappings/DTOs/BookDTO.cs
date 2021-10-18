using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Personal_EF_API.Data.Mappings.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
        public string Isbn { get; set; }

        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public int? AuthorId { get; set; }
        public virtual AuthorDTO Author { get; set; }
    }
    public class CreateBookDTO
    {
        [Required]
        public string Title { get; set; }
        public int? Year { get; set; }
        public string Isbn { get; set; }
        [StringLength(200)]
        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        [Required]
        public int? AuthorId { get; set; }       

    }
    public class UpdateBookDTO
    {

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int? Year { get; set; }
      
        [StringLength(200)]
        public string Summary { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }

        public int? AuthorId { get; set; }

    }
}
