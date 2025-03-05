using ApiTask.DataInfrastructure.Entities.Enum;
using ApiTask.Dto.In;
using ApiTask.Dto.Out;
using ApiTask.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [JwtUserIdFilter]
    public class TaskController(ITaskService taskService) : ControllerBase
    {
        private readonly ITaskService _taskService = taskService;

        /// <summary>
        /// Busca uma lista de tarefas com filstro de status
        /// </summary>
        /// <param name="status"></param>
        /// <returns>Retorna uma lista de tarefas</returns>
        [HttpGet("List/{status?}")]
        [ProducesResponseType(typeof(TaskOutDto[]), 200)]
        public async Task<IActionResult> GetList(Status? status)
            => Ok(await _taskService.GetList(status));

        /// <summary>
        /// Busca a tarefa pelo Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Retorna a tarefa</returns>
        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(TaskOutDto), 200)]
        public async Task<IActionResult> GetById(string Id)
            => Ok(await _taskService.GetById(Id));

        /// <summary>
        /// Cria Tarefa
        /// </summary>
        /// <param name="taskCreateDto">Model de entrada</param>
        /// <returns>Retorna verdadeiro se salvo corretamente</returns>
        [HttpPost]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> Create(TaskInDto taskCreateDto)
            => Ok(await _taskService.Create(taskCreateDto, GetUserId()));
        

        /// <summary>
        /// Atualiza tarefa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskUpdateDto">Model de entrada</param>
        /// <returns>Retorna verdadeiro se atualizou com sucesso</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> Update(string id, TaskInDto taskUpdateDto)
            => Ok(await _taskService.Update(id, taskUpdateDto, GetUserId()));

        /// <summary>
        /// Deleta tarefa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskUpdateDto">Model de entrada</param>
        /// <returns>Retorna verdadeiro se atualizou com sucesso</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> Delete(string id)
            => Ok(await _taskService.Delete(id, GetUserId()));

        private string GetUserId()
            => HttpContext.Items["UserId"]?.ToString() ?? throw new UnauthorizedAccessException();
    }
}
