using System.ComponentModel.DataAnnotations;

namespace Edveeeeeee.Models
{
    public class Avaliacao
    {
        [Key]
        public int Id { get; set; }
        public string Texto { get; set; }

        public int UnidadeCurricularId { get; set; }
        public UnidadeCurricular UnidadeCurricular { get; set; }
    }
}
