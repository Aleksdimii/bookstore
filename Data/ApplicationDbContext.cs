using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bookstore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Author> Author { get; set; } = default!;
        public DbSet<Book> Book { get; set; } = default!;
        public DbSet<Order> Order { get; set; } = default!;
    }
}
