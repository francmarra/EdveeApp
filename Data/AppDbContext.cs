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

        public DbSet<Competencia> Competencias { get; set; }
        public DbSet<Conteudo> Conteudos { get; set; }
        public DbSet<Atividade> Atividades { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }

    }
}
