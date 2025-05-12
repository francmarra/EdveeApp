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
                
            };

            context.UCs.AddRange(ucs);
            context.SaveChanges();
        }
    }
}
