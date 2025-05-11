using System.Collections.Generic;

namespace Edveeeeeee.Models.ViewModels
{
    public class NovaLigacaoViewModel
    {
        public int UnidadeCurricularId { get; set; }

        public List<Competencia> Competencias { get; set; }
        public List<Conteudo> Conteudos { get; set; }
        public List<Atividade> Atividades { get; set; }
        public List<Avaliacao> Avaliacoes { get; set; }

        public string OrigemTipo { get; set; }
        public int OrigemId { get; set; }

        public string DestinoTipo { get; set; }
        public int DestinoId { get; set; }
    }
}