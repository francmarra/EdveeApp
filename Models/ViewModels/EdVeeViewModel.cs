using Microsoft.AspNetCore.Mvc;

namespace Edveeeeeee.Models.ViewModels
{
    public class EdVeeViewModel 
    {
        public UnidadeCurricular UnidadeCurricular { get; set; }

        public List<Competencia> Competencias { get; set; }
        public List<Conteudo> Conteudos { get; set; }
        public List<Atividade> Atividades { get; set; }
        public List<Avaliacao> Avaliacoes { get; set; }

        public List<LigacaoEdVee> Ligacoes { get; set; }
    }
}
