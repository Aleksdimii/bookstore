using AutoMapper;
using bookstore.Data;     
using bookstore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookViewModel>().ReverseMap();
        CreateMap<Author, AuthorViewModel>().ReverseMap();
    }
}
