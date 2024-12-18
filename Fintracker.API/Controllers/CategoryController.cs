using Fintracker.Application.DTO.Category;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Features.Category.Requests.Queries;
using Fintracker.Application.Models;
using Fintracker.Application.Responses.API_Responses;
using Fintracker.Application.Responses.Commands_Responses;
using Fintracker.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[Route("api/category")]
[Authorize(Roles = "Admin, User")]
public class CategoryController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _environment;

    public CategoryController(IMediator mediator, IWebHostEnvironment environment)
    {
        _mediator = mediator;
        _environment = environment;
    }


    [HttpGet("user/{userId:guid?}")]
    [ProducesResponseType(typeof(List<CategoryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<CategoryDTO>>> Get(Guid? userId, [FromQuery] QueryParams query)
    {
        var sortRequest = new GetCategoriesSortedRequest
        {
            Params = query,
            UserId = userId ?? GetCurrentUserId()
        };

        var simpleRequest = new GetCategoriesRequest
        {
            UserId = userId ?? GetCurrentUserId()
        };

        IReadOnlyList<CategoryDTO> response;

        if (!string.IsNullOrEmpty(query.SortBy))
            response = await _mediator.Send(sortRequest);
        else
            response = await _mediator.Send(simpleRequest);

        return Ok(response);
    }


    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDTO>> Get(Guid id)
    {
        var response = await _mediator.Send(new GetCategoryByIdRequest
        {
            Id = id,
            UserId = GetCurrentUserId()
        });

        return Ok(response);
    }

    [HttpGet("{type}/user/{userId:guid?}")]
    [ProducesResponseType(typeof(List<CategoryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<CategoryDTO>>> Get(Guid? userId, CategoryType type)
    {
        var response = await _mediator.Send(new GetCategoriesByTypeRequest
        {
            Type = type,
            UserId = userId ?? GetCurrentUserId()
        });

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateCommandResponse<CategoryDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateCommandResponse<CategoryDTO>>> Post([FromBody] CreateCategoryDTO category)
    {
        var response = await _mediator.Send(new CreateCategoryCommand
        {
            Category = category,
            UserId = GetCurrentUserId()
        });

        return Ok(response);
    }

    [HttpPost("populate")]
    [ProducesResponseType(typeof(CreateCommandResponse<CategoryDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Pupulate()
    {
        var response = await _mediator.Send(new PopulateUserWithCategoriesCommand()
        {
            UserId = GetCurrentUserId(),
            PathToFile = Path.Combine(_environment.WebRootPath, "data", "categories.json")
        });

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(typeof(UpdateCommandResponse<CategoryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateCommandResponse<CategoryDTO>>> Put([FromBody] UpdateCategoryDTO category)
    {
        var response = await _mediator.Send(new UpdateCategoryCommand
        {
            Category = category,
            UserId = GetCurrentUserId()
        });

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(DeleteCommandResponse<CategoryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteCommandResponse<CategoryDTO>>> Delete(
        Guid id,
        Guid? categoryToReplaceId
    )
    {
        var response = await _mediator.Send(new DeleteCategoryCommand
        {
            Id = id,
            UserId = GetCurrentUserId(),
            CategoryToReplaceId = categoryToReplaceId,
        });

        return Ok(response);
    }
}