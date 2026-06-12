using Microsoft.AspNetCore.Mvc;

namespace TasksApi.Controllers;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    // Datos en memoria compartidos entre requests; suficiente para el workshop
    private static readonly List<TaskItem> Tasks = new()
    {
        new TaskItem { Id = 1, Title = "Revisar signos vitales del paciente #12", IsCompleted = false },
        new TaskItem { Id = 2, Title = "Registrar consulta de pediatría pendiente", IsCompleted = false },
        new TaskItem { Id = 3, Title = "Actualizar expediente del paciente #34", IsCompleted = false },
        new TaskItem { Id = 4, Title = "Preparar medicamentos del turno de la tarde", IsCompleted = false },
        new TaskItem { Id = 5, Title = "Confirmar cita de laboratorio del paciente #7", IsCompleted = false }
    };

    [HttpGet]
    public ActionResult<IEnumerable<TaskItem>> GetTasks()
    {
        return Ok(Tasks);
    }

    [HttpPatch("{id}/complete")]
    public ActionResult<TaskItem> CompleteTask(int id)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);
        if (task is null)
        {
            return NotFound();
        }

        task.IsCompleted = true;
        return Ok(task);
    }
}
