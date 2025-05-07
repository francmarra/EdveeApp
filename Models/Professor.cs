using System.ComponentModel.DataAnnotations;
namespace Edveeeeeee.Models
{
    public class Professor
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<UnidadeCurricular> UCs { get; set; }
    }
}
