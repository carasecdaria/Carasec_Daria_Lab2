using Carasec_Daria_Lab2.Data;

using Carasec_Daria_Lab2.Models;
using Microsoft.EntityFrameworkCore;

namespace Carasec_Daria_Lab2.Data
{
    public class Carasec_Daria_Lab2Context : DbContext
    {
        public Carasec_Daria_Lab2Context(
            DbContextOptions<Carasec_Daria_Lab2Context> options
        ) : base(options)
        {
        }

        public DbSet<Book> Book { get; set; } = default!;

        public DbSet<Publisher> Publisher { get; set; } = default!;

        public DbSet<Author> Author { get; set; } = default!;
    }
}
