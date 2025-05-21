using System.Collections.Generic;

namespace Edveeeeeee.Models.ViewModels
{
    public class SaveConnectionsViewModel
    {
        public int ucId { get; set; }
        public List<ConnectionViewModel> connections { get; set; }
    }

    public class ConnectionViewModel
    {
        public string origemTipo { get; set; }
        public string origemId { get; set; }
        public string destinoTipo { get; set; }
        public string destinoId { get; set; }
    }
}
