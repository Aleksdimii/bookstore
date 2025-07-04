using bookstore.Controllers;
using bookstore.Data;
using bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;

public class AuthorsControllerTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("AuthorsTestDb")
            .Options;

        var context = new ApplicationDbContext(options);

        if (!context.Author.Any())
        {
            context.Author.Add(new Author
            {
                AuthorId = 1,
                Name = "Test Author",
                Biography = "Some biography"
            });
            context.SaveChanges();
        }

        return context;
    }

    [Fact]
    public async Task Index_ReturnsViewWithAuthors()
    {
        var context = GetDbContext();

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<List<AuthorViewModel>>(It.IsAny<List<Author>>()))
            .Returns(new List<AuthorViewModel>
            {
                new AuthorViewModel { AuthorId = 1, Name = "Test Author" }
            });

        var controller = new AuthorsController(context, mapperMock.Object);

        var result = await controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<AuthorViewModel>>(viewResult.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task Details_ReturnsNotFound_WhenIdIsNull()
    {
        var controller = new AuthorsController(GetDbContext(), new Mock<IMapper>().Object);

        var result = await controller.Details(null);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_ReturnsNotFound_WhenAuthorNotFound()
    {
        var controller = new AuthorsController(GetDbContext(), new Mock<IMapper>().Object);

        var result = await controller.Details(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
