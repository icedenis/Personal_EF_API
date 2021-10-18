using AutoMapper;
using Personal_EF_API.Data.Mappings.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personal_EF_API.Data.Mappings
{
    public class Maps : Profile
    {
     public Maps()
        {
            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<Book, BookDTO>().ReverseMap();
            // thats for the Create new Author in the Dta transfer objects
            CreateMap<Author, CreatAuthorDTO>().ReverseMap();
            CreateMap<Author, UpdateAuthorDTO>().ReverseMap();
        }
    }
}
