using AutoMapper;
using bookstore.Models;
using bookstore.Web.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace bookstore.Web.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookViewModel>()
                .ForMember(dest => dest.AuthorNames, opt =>
                    opt.MapFrom(src => src.Authors.Select(a => a.Name)));

            CreateMap<BookViewModel, Book>()
                .ForMember(dest => dest.Authors, opt => opt.Ignore());

            CreateMap<Author, AuthorViewModel>();
            CreateMap<AuthorViewModel, Author>();

            CreateMap<Order, OrderViewModel>()
                .ForMember(dest => dest.BookTitles, opt =>
                    opt.MapFrom(src => src.Books.Select(b => b.Title)));

            CreateMap<OrderViewModel, Order>()
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }
    }
}
