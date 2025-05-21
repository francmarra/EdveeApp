using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edveeeeeee.Models
{
    public class UnidadeCurricular
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Codigo { get; set; }

        [Required]
        public string Turmas { get; set; }

        public string Descricao { get; set; } 
        //ola

        // Relações com componentes EdVee
        public virtual ICollection<Competencia> Competencias { get; set; } = new List<Competencia>();
        public virtual ICollection<Conteudo> Conteudos { get; set; } = new List<Conteudo>();
        public virtual ICollection<Atividade> Atividades { get; set; } = new List<Atividade>();
        public virtual ICollection<Avaliacao> Avaliacao { get; set; } = new List<Avaliacao>();

        public int ProfessorId { get; set; }

    }
}
