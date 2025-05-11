using System.ComponentModel.DataAnnotations;

namespace Edveeeeeee.Models
{
    public class LigacaoEdVee
    {
        [Key]
        public int Id { get; set; }

        public int UnidadeCurricularId { get; set; }
        public UnidadeCurricular UnidadeCurricular { get; set; }

        public string OrigemTipo { get; set; } // "Competencia", "Conteudo"
        public int OrigemId { get; set; }

        public string DestinoTipo { get; set; } // "Atividade", "Avaliacao"
        public int DestinoId { get; set; }
    }
}