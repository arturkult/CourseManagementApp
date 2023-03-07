using Application.Courses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class CourseController : MediatorControllerBase
{
    public CourseController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Creates new course
    /// </summary>
    /// <param name="command">Name, description and list of topics</param>
    [HttpPost]
    public Task<IActionResult> Create(AddCourseCommand command, CancellationToken token = default)
        => Send(command, token);

    /// <summary>
    /// Updates existing course
    /// </summary>
    /// <param name="command">Name, description and list of topics</param>
    /// <param name="id">Course id</param>
    [HttpPut("{id:guid}")]
    public Task<IActionResult> Update(Guid id, UpdateCourseCommand command, CancellationToken token = default)
        => Send(command with { Id = id }, token);

    /// <summary>
    /// Deletes existing course
    /// </summary>
    /// <param name="id">Course id to delete</param>
    [HttpDelete("{id:guid}")]
    public Task<IActionResult> Delete(Guid id, CancellationToken token = default)
        => Send(new DeleteCourseCommand(id), token);

    /// <summary>
    /// Browse courses
    /// </summary>
    /// <param name="id">Course id to delete</param>
    [HttpGet]
    public Task<IActionResult> Browse([FromQuery] BrowseCoursesQuery query, CancellationToken token = default)
        => Send(query, token);
    
    /// <summary>
    /// Browse courses
    /// </summary>
    /// <param name="id">Course id to delete</param>
    [HttpGet("{id:guid}")]
    public Task<IActionResult> Browse(Guid id, CancellationToken token = default)
        => Send(new GetCourseByIdQuery(id), token);
}