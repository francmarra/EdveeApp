using Edveeeeeee.Models;

namespace Edveeeeeee.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Só corre se a base estiver vazia
            if (context.Professores.Any()) return;

            var paulo = new Professor { Nome = "Paulo", Username = "paulo", Password = "123" };
            var maria = new Professor { Nome = "Maria", Username = "maria", Password = "123" };

            context.Professores.AddRange(paulo, maria);
            context.SaveChanges(); // guardar para que os IDs fiquem disponíveis

            var ucs = new List<UnidadeCurricular>
            {
                new UnidadeCurricular { Nome = "Programação Web", Codigo = "14215", Turmas = "PL3, T1", Regime = "1.º Semestre", ProfessorId = paulo.Id },
                new UnidadeCurricular { Nome = "Sistemas Distribuídos", Codigo = "14220", Turmas = "PL4, T1", Regime = "2.º Semestre", ProfessorId = paulo.Id },

                new UnidadeCurricular { Nome = "Análise Matemática I", Codigo = "20232", Turmas = "T2, TP4", Regime = "1.º Semestre", ProfessorId = maria.Id },
                new UnidadeCurricular { Nome = "Métodos Estatísticos", Codigo = "14276", Turmas = "PL6, T1", Regime = "1.º Semestre", ProfessorId = maria.Id }
            };

            context.UCs.AddRange(ucs);
            context.SaveChanges();
        }
    }
}
