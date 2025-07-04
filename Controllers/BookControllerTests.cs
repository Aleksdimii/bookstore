using bookstore.Controllers;
using bookstore.Data;
using bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;
using AutoMapper;

public class BooksControllerTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        var context = new ApplicationDbContext(options);

        if (!context.Book.Any())
        {
            context.Book.Add(new Book
            {
                BookId = 1,
                Title = "Test Book",
                ISBN = "1234567890",
                Price = 10
            });
            context.SaveChanges();
        }

        return context;
    }

    [Fact]
    public async Task Index_ReturnsViewWithBooks()
    {
        var context = GetDbContext();

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<List<BookViewModel>>(It.IsAny<List<Book>>()))
            .Returns(new List<BookViewModel>
            {
                new BookViewModel { BookId = 1, Title = "Test Book" }
            });

        var controller = new BooksController(context, mapperMock.Object);

        var result = await controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<BookViewModel>>(viewResult.Model);
        Assert.Single(model);
    }
}
