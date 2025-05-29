using Microsoft.AspNetCore.Mvc;
using Edveeeeeee.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Edveeeeeee.Models;
using Edveeeeeee.Models.ViewModels;
using System.Text.Json.Serialization;

namespace Edveeeeeee.Controllers
{
    public class UCController : Controller
    {
        private readonly AppDbContext _context;

        public UCController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("userId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Conta");
            }

            var ucs = _context.UCs
                .Where(uc => uc.ProfessorId == userId)
                .ToList();

            return View(ucs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UnidadeCurricular uc, string competencias, string conteudos, string atividades, string avaliacao)
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null) return RedirectToAction("Login", "Conta");

            uc.ProfessorId = (int)userId;
            _context.UCs.Add(uc);
            _context.SaveChanges(); // agora temos o ID da UC

            // Processar campos EdVee (um por linha)
            var compList = competencias?.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList() ?? new();
            var contList = conteudos?.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList() ?? new();
            var ativList = atividades?.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList() ?? new();
            var avalList = avaliacao?.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToList() ?? new();

            _context.Competencias.AddRange(compList.Select(c => new Competencia { Texto = c.Trim(), UnidadeCurricularId = uc.Id }));
            _context.Conteudos.AddRange(contList.Select(c => new Conteudo { Texto = c.Trim(), UnidadeCurricularId = uc.Id }));
            _context.Atividades.AddRange(ativList.Select(a => new Atividade { Texto = a.Trim(), UnidadeCurricularId = uc.Id }));
            _context.Avaliacoes.AddRange(avalList.Select(a => new Avaliacao { Texto = a.Trim(), UnidadeCurricularId = uc.Id }));

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddCompetencia(int ucId, string texto)
        {
            if (!string.IsNullOrWhiteSpace(texto))
            {
                _context.Competencias.Add(new Competencia { Texto = texto.Trim(), UnidadeCurricularId = ucId });
                _context.SaveChanges();
            }

            return RedirectToAction("EdVee", new { id = ucId });
        }

        public IActionResult EditCompetencia(int id)
        {
            var competencia = _context.Competencias.FirstOrDefault(c => c.Id == id);
            if (competencia == null) return NotFound();
            return View(competencia);
        }

        [HttpPost]
        public IActionResult EditCompetencia(Competencia model)
        {
            _context.Competencias.Update(model);
            _context.SaveChanges();
            return RedirectToAction("EdVee", new { id = model.UnidadeCurricularId });
        }

        [HttpPost]
        public IActionResult DeleteCompetencia(int id)
        {
            var comp = _context.Competencias.FirstOrDefault(c => c.Id == id);
            if (comp != null)
            {
                int ucId = comp.UnidadeCurricularId;
                _context.Competencias.Remove(comp);
                _context.SaveChanges();
                return RedirectToAction("EdVee", new { id = ucId });
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult AddConteudo(int ucId, string texto)
        {
            if (!string.IsNullOrWhiteSpace(texto))
            {
                _context.Conteudos.Add(new Conteudo
                {
                    Texto = texto.Trim(),
                    UnidadeCurricularId = ucId
                });
                _context.SaveChanges();
            }

            return RedirectToAction("EdVee", new { id = ucId });
        }

        public IActionResult EditConteudo(int id)
        {
            var conteudo = _context.Conteudos.FirstOrDefault(c => c.Id == id);
            if (conteudo == null) return NotFound();
            return View(conteudo);
        }

        [HttpPost]
        public IActionResult EditConteudo(Conteudo model)
        {
            _context.Conteudos.Update(model);
            _context.SaveChanges();
            return RedirectToAction("EdVee", new { id = model.UnidadeCurricularId });
        }

        [HttpPost]
        public IActionResult DeleteConteudo(int id)
        {
            var conteudo = _context.Conteudos.FirstOrDefault(c => c.Id == id);
            if (conteudo != null)
            {
                int ucId = conteudo.UnidadeCurricularId;
                _context.Conteudos.Remove(conteudo);
                _context.SaveChanges();
                return RedirectToAction("EdVee", new { id = ucId });
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult AddAtividade(int ucId, string texto)
        {
            if (!string.IsNullOrWhiteSpace(texto))
            {
                _context.Atividades.Add(new Atividade
                {
                    Texto = texto.Trim(),
                    UnidadeCurricularId = ucId
                });
                _context.SaveChanges();
            }

            return RedirectToAction("EdVee", new { id = ucId });
        }

        public IActionResult EditAtividade(int id)
        {
            var atividade = _context.Atividades.FirstOrDefault(a => a.Id == id);
            if (atividade == null) return NotFound();
            return View(atividade);
        }

        [HttpPost]
        public IActionResult EditAtividade(Atividade model)
        {
            _context.Atividades.Update(model);
            _context.SaveChanges();
            return RedirectToAction("EdVee", new { id = model.UnidadeCurricularId });
        }

        [HttpPost]
        public IActionResult DeleteAtividade(int id)
        {
            var atividade = _context.Atividades.FirstOrDefault(a => a.Id == id);
            if (atividade != null)
            {
                int ucId = atividade.UnidadeCurricularId;
                _context.Atividades.Remove(atividade);
                _context.SaveChanges();
                return RedirectToAction("EdVee", new { id = ucId });
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult AddAvaliacao(int ucId, string texto)
        {
            if (!string.IsNullOrWhiteSpace(texto))
            {
                _context.Avaliacoes.Add(new Avaliacao
                {
                    Texto = texto.Trim(),
                    UnidadeCurricularId = ucId
                });
                _context.SaveChanges();
            }

            return RedirectToAction("EdVee", new { id = ucId });
        }

        public IActionResult EditAvaliacao(int id)
        {
            var avaliacao = _context.Avaliacoes.FirstOrDefault(a => a.Id == id);
            if (avaliacao == null) return NotFound();
            return View(avaliacao);
        }

        [HttpPost]
        public IActionResult EditAvaliacao(Avaliacao model)
        {
            _context.Avaliacoes.Update(model);
            _context.SaveChanges();
            return RedirectToAction("EdVee", new { id = model.UnidadeCurricularId });
        }

        [HttpPost]
        public IActionResult DeleteAvaliacao(int id)
        {
            var avaliacao = _context.Avaliacoes.FirstOrDefault(a => a.Id == id);
            if (avaliacao != null)
            {
                int ucId = avaliacao.UnidadeCurricularId;
                _context.Avaliacoes.Remove(avaliacao);
                _context.SaveChanges();
                return RedirectToAction("EdVee", new { id = ucId });
            }
            return NotFound();
        }




        public IActionResult Edit(int id)
        {
            var uc = _context.UCs.FirstOrDefault(x => x.Id == id);
            if (uc == null) return NotFound();
            return View(uc);
        }        [HttpPost]
        public IActionResult Edit(UnidadeCurricular uc)
        {
            var existingUc = _context.UCs.FirstOrDefault(u => u.Id == uc.Id);
            if (existingUc == null) return NotFound();

            // Update only the fields that should be editable, preserving ProfessorId
            existingUc.Nome = uc.Nome;
            existingUc.Codigo = uc.Codigo;
            existingUc.Turmas = uc.Turmas;
            existingUc.Descricao = uc.Descricao;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult EdVee(int id)
        {
            var uc = _context.UCs.FirstOrDefault(u => u.Id == id);
            if (uc == null) return NotFound();

            var model = new EdVeeViewModel
            {
                UnidadeCurricular = uc,
                Competencias = _context.Competencias
                    .Where(c => c.UnidadeCurricularId == id).ToList(),

                Conteudos = _context.Conteudos
                    .Where(c => c.UnidadeCurricularId == id).ToList(),

                Atividades = _context.Atividades
                    .Where(a => a.UnidadeCurricularId == id).ToList(),

                Avaliacoes = _context.Avaliacoes
                    .Where(a => a.UnidadeCurricularId == id).ToList(),

                Ligacoes = _context.Ligacoes
                    .Where(l => l.UnidadeCurricularId == id).ToList()
            };

            return View(model);
        }



        public IActionResult EditUC(int id)
        {
            var uc = _context.UCs.FirstOrDefault(u => u.Id == id);
            if (uc == null) return NotFound();
            return View(uc);
        }

        [HttpPost]
        public IActionResult EditUC(UnidadeCurricular model)
        {
            var uc = _context.UCs.FirstOrDefault(u => u.Id == model.Id);
            if (uc == null) return NotFound();

            // Atualizar apenas os campos simples
            uc.Nome = model.Nome;
            uc.Codigo = model.Codigo;
            uc.Turmas = model.Turmas;
            uc.Descricao = model.Descricao;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteUC(int id)
        {
            var uc = _context.UCs.FirstOrDefault(u => u.Id == id);
            if (uc != null)
            {
                // Remover os blocos EdVee relacionados
                var comps = _context.Competencias.Where(c => c.UnidadeCurricularId == id);
                var conts = _context.Conteudos.Where(c => c.UnidadeCurricularId == id);
                var ativs = _context.Atividades.Where(a => a.UnidadeCurricularId == id);
                var avls = _context.Avaliacoes.Where(a => a.UnidadeCurricularId == id);

                _context.Competencias.RemoveRange(comps);
                _context.Conteudos.RemoveRange(conts);
                _context.Atividades.RemoveRange(ativs);
                _context.Avaliacoes.RemoveRange(avls);

                // Remover a UC
                _context.UCs.Remove(uc);
                _context.SaveChanges();
            }            return RedirectToAction("Index");
        }        
        
        // O método NovaLigacao(int id) foi removido porque agora usamos as ligações interativas na tela principal
        // O método NovaLigacao foi removido porque agora usamos as ligações interativas na tela principal
        // O método EdVeeMatriz foi removido porque agora usamos uma única visualização interativa

        [HttpGet]
        public IActionResult GetConnections(int id)
        {
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null) return Unauthorized();
            
            // Busca todas as ligações da UC específica
            var ligacoes = _context.Ligacoes
                .Where(l => l.UnidadeCurricularId == id)
                .Select(l => new {
                    origemTipo = l.OrigemTipo,
                    origemId = l.OrigemId,
                    destinoTipo = l.DestinoTipo,
                    destinoId = l.DestinoId
                })
                .ToList();
            
            return Json(ligacoes);
        }

        [HttpPost]
        public IActionResult SaveConnections([FromBody] SaveConnectionsViewModel model)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("userId");
                if (userId == null)
                    return Unauthorized(new { success = false, message = "Utilizador não autenticado" });

                // Verifica se a UC pertence ao professor atual
                var uc = _context.UCs.FirstOrDefault(u => u.Id == model.ucId && u.ProfessorId == userId);
                if (uc == null)
                    return NotFound(new { success = false, message = "Unidade Curricular não encontrada" });

                // Adiciona log para depuração
                System.Diagnostics.Debug.WriteLine($"Salvando {model.connections?.Count ?? 0} conexões para UC {model.ucId}");

                // Remove todas as ligações existentes para esta UC
                var existingConnections = _context.Ligacoes.Where(l => l.UnidadeCurricularId == model.ucId).ToList();
                if (existingConnections.Any())
                {
                    _context.Ligacoes.RemoveRange(existingConnections);
                    _context.SaveChanges(); // Salva a remoção primeiro
                }

                // Adiciona as novas ligações, se houver
                if (model.connections != null && model.connections.Any())
                {
                    foreach (var conn in model.connections)
                    {
                        // Verificamos se os campos origemId e destinoId podem ser convertidos para int
                        if (int.TryParse(conn.origemId, out int origemId) &&
                            int.TryParse(conn.destinoId, out int destinoId))
                        {
                            var ligacao = new LigacaoEdVee 
                            {
                                UnidadeCurricularId = model.ucId,
                                OrigemTipo = conn.origemTipo,
                                OrigemId = origemId,
                                DestinoTipo = conn.destinoTipo,
                                DestinoId = destinoId
                            };
                            
                            _context.Ligacoes.Add(ligacao);
                            System.Diagnostics.Debug.WriteLine($"Adicionada ligação: {conn.origemTipo}-{origemId} -> {conn.destinoTipo}-{destinoId}");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"Erro de formato: origemId={conn.origemId}, destinoId={conn.destinoId}");
                        }
                    }
                    
                    _context.SaveChanges(); // Importante: salva as novas conexões
                }

                return Ok(new { success = true, message = "Conexões salvas com sucesso" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao salvar conexões: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Erro ao salvar conexões", error = ex.Message });
            }
        }
    }
}
