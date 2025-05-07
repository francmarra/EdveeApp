using System.ComponentModel.DataAnnotations;

namespace Edveeeeeee.Models
{
    public class UnidadeCurricular
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }
        public string Codigo { get; set; }
        public string Turmas { get; set; }
        public string Regime { get; set; }

        public int ProfessorId { get; set; }
        public Professor Professor { get; set; }
    }
}
