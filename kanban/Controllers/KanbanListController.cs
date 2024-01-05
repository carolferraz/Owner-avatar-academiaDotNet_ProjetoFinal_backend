using Castle.Core.Internal;
using kanban.DataModels;
using kanban.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace kanban.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : Controller
    {
        private readonly kanbanContext _context;

        public ListController(kanbanContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLists()
        {
            try
            {
                var lists = await _context.Lists.Include(l => l.KanbanTasks).ToListAsync();
                return Ok(lists);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{listId}")]
        public async Task<IActionResult> GetListById([FromRoute] int listId)
        {
            try
            {
                var list = await _context.Lists
                            .Include(l => l.KanbanTasks)
                            .FirstOrDefaultAsync(l => l.Id == listId);

                if (list == null)
                    return NotFound("Lista não encontrada.");

                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateList([FromBody] List newList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                newList.CreatedOn = DateTime.UtcNow;
                newList.ModifiedOn = DateTime.UtcNow;

                _context.Lists.Add(newList);
                await _context.SaveChangesAsync();

                // Construa a URL manualmente para o novo recurso criado
                string locationUrl = $"{Request.Scheme}://{Request.Host}/api/List/{newList.Id}";

                // Retorna um código de status 201 (Created) e um cabeçalho 'Location' com a URL do novo recurso
                return Ok(newList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{listId}")]
        public async Task<IActionResult> UpdateList([FromRoute] int listId, [FromBody] List updatedList)
        {
            try
            {
                var list = await _context.Lists.FirstOrDefaultAsync(l => l.Id == listId);
                if (list == null)
                    return NotFound("Lista não encontrada para o Board especificado.");

                list.Title = updatedList.Title;

                _context.Lists.Update(list);
                await _context.SaveChangesAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{listId}")]
        public async Task<IActionResult> DeleteList([FromRoute] int listId)
        {
            try
            {
                var list = await _context.Lists.Include(l => l.KanbanTasks).FirstOrDefaultAsync(l => l.Id == listId);
                if (list == null)
                    return NotFound("Lista não encontrada.");

                _context.KanbanTasks.RemoveRange(list.KanbanTasks); // Remove todas as tasks associadas à lista

                _context.Lists.Remove(list); // Remove a lista em si
                await _context.SaveChangesAsync();


                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        ///// Métodos para Tarefas (KanbanTasks) ////

        [HttpGet("{listId}/tasks")]
        public async Task<IActionResult> GetAllKanbanTasksByListId([FromRoute] int listId)
        {
            try
            {
                var tasks = await _context.KanbanTasks
                    .Where(t => t.ListId == listId)
                    .ToListAsync();

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/tasks/{taskId}")]
        public async Task<IActionResult> GetKanbanTaskById(int listId, [FromRoute] int taskId)
        {
            try
            {
                var task = await _context.KanbanTasks
                    .FirstOrDefaultAsync(t => t.Id == taskId && t.ListId == listId);

                if (task == null)
                    return NotFound("Tarefa não encontrada para a Lista.");

                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{listId}/tasks")]
        public async Task<IActionResult> CreateKanbanTask([FromRoute] int listId, [FromBody] KanbanTask newKanbanTask)
        {
            try
            {
                var list = await _context.Lists.FirstOrDefaultAsync(l => l.Id == listId);

                if (list == null)
                    return NotFound("Lista não encontrada.");

                newKanbanTask.ListId = listId;
                //newKanbanTask.List = list;
                newKanbanTask.CreatedOn = DateTime.UtcNow;

                _context.KanbanTasks.Add(newKanbanTask);
                await _context.SaveChangesAsync();


                // Constrói a URL manualmente
                string locationUrl = $"{Request.Scheme}://{Request.Host}/api/List/{listId}/tasks/{newKanbanTask.Id}";

                // Retorna um código de status 201 (Created) e um cabeçalho 'Location' com a URL do novo recurso
                return Ok(newKanbanTask);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{listId}/tasks/{taskId}")]
        public async Task<IActionResult> UpdateKanbanTask([FromRoute] int listId, [FromRoute] int taskId, [FromBody] KanbanTask updatedKanbanTask)
        {
            try
            {
                var task = await _context.KanbanTasks
                    .FirstOrDefaultAsync(t => t.Id == taskId && t.ListId == listId);

                if (task == null)
                    return NotFound("Tarefa não encontrada para a Lista especificada.");

                task.Title = updatedKanbanTask.Title;
                task.Description = updatedKanbanTask.Description;
                task.Priority = updatedKanbanTask.Priority;
                task.ModifiedOn = DateTime.UtcNow;

                _context.KanbanTasks.Update(task);
                await _context.SaveChangesAsync();

                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{listId}/tasks/{taskId}")]
        public async Task<IActionResult> DeleteKanbanTask(int listId, int taskId)
        {
            try
            {
                var task = await _context.KanbanTasks
                    .FirstOrDefaultAsync(t => t.Id == taskId && t.ListId == listId);

                if (task == null)
                    return NotFound("Tarefa não encontrada para a Lista especificada.");

                _context.KanbanTasks.Remove(task);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


