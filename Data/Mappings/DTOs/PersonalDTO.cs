using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Personal_EF_API.Data.Mappings.DTOs
{
    public class PersonalDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(15,ErrorMessage ="Max 15 char",MinimumLength =8)]
        public string Password { get; set; }

        public string Email { get; set; }
        // can add emeil here
    }
}
