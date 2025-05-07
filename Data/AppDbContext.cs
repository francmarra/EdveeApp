using Microsoft.EntityFrameworkCore;
using Edveeeeeee.Models;

namespace Edveeeeeee.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Professor> Professores { get; set; }
        public DbSet<UnidadeCurricular> UCs { get; set; }
    }
}
