using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _db;

        public TarefaController(OrganizadorContext context)
        {
            _db = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id, CancellationToken ct)
        {
            // TODO: Buscar o Id no banco utilizando o EF
            // TODO: Validar o tipo de retorno. Se não encontrar a tarefa, retornar NotFound,
            // caso contrário retornar OK com a tarefa encontrada
            var tarefa = await _db.Tarefas.FirstOrDefaultAsync(f => f.Id == id, ct);
            if(tarefa is null)
                return NotFound();

            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public async Task<IActionResult> ObterTodos(CancellationToken ct)
        {
            // TODO: Buscar todas as tarefas no banco utilizando o EF
            var tarefas = await _db.Tarefas.ToListAsync(ct);

            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public async Task<IActionResult> ObterPorTitulo(string titulo, CancellationToken ct)
        {
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o titulo recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
            var tarefas = await _db.Tarefas.Where(f => f.Titulo == titulo).ToListAsync(ct);
            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public async Task<IActionResult> ObterPorData(DateTime data, CancellationToken ct)
        {
            var tarefa = await _db.Tarefas.Where(x => x.Data.Date == data.Date).ToListAsync(ct);
            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public async Task<IActionResult> ObterPorStatus(EnumStatusTarefa status, CancellationToken ct)
        {
            // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
            // Dica: Usar como exemplo o endpoint ObterPorData
            var tarefa = await _db.Tarefas.Where(x => x.Status == status).ToListAsync(ct);
            return Ok(tarefa);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Tarefa tarefa, CancellationToken ct)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // TODO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            _db.Tarefas.Add(tarefa);
            await _db.SaveChangesAsync(ct);
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Tarefa tarefa, CancellationToken ct)
        {
            var tarefaBanco = await _db.Tarefas.FirstOrDefaultAsync(f => f.Id == id, ct);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Status = tarefa.Status;

            await _db.SaveChangesAsync(ct);

            // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            // TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id, CancellationToken ct)
        {
            var tarefaBanco = await _db.Tarefas.FirstOrDefaultAsync(f => f.Id == id, ct); ;

            if (tarefaBanco == null)
                return NotFound();

            _db.Remove(tarefaBanco);
            await _db.SaveChangesAsync(ct);
            // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            return NoContent();
        }
    }
}
