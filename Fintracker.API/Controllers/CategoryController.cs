using Fintracker.Application.DTO.Category;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Features.Category.Requests.Queries;
using Fintracker.Application.Responses;
using Fintracker.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDTO>>> Get([FromQuery] string sortBy, [FromQuery] bool isDescending = false)
    {
        var sortRequest = new GetCategoriesSortedRequest()
        {
            IsDescending = isDescending,
            SortBy = sortBy
        };

        var simpleRequest = new GetCategoriesRequest();

        IReadOnlyList<CategoryDTO> response;
        
        if (!string.IsNullOrEmpty(sortBy))
            response = await _mediator.Send(sortRequest);
        else
            response = await _mediator.Send(simpleRequest);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryDTO>> Get(Guid id)
    {
        var response = await _mediator.Send(new GetCategoryByIdRequest()
        {
            Id = id
        });

        return Ok(response);
    }
    
    [HttpGet("{type}")]
    public async Task<ActionResult<List<CategoryDTO>>> Get(CategoryType type)
    {
        var response = await _mediator.Send(new GetCategoriesByTypeRequest()
        {
            Type = type
        });

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CreateCommandResponse<CategoryDTO>>> Post([FromBody] CreateCategoryDTO category)
    {
        var response = await _mediator.Send(new CreateCategoryCommand()
        {
            Category = category
        });

        return Ok(response);
    }
    
    [HttpPut]
    public async Task<ActionResult<UpdateCommandResponse<CategoryDTO>>> Put([FromBody] UpdateCategoryDTO category)
    {
        var response = await _mediator.Send(new UpdateCategoryCommand()
        {
            Category = category
        });

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<DeleteCommandResponse<CategoryDTO>>> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteCategoryCommand()
        {
            Id = id
        });

        return Ok(response);
    }
}