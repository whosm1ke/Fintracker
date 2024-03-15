using Fintracker.Application.BusinessRuleConstraints;
using Fintracker.Application.DTO.Category;
using Fintracker.Application.Features.Category.Requests.Commands;
using Fintracker.Application.Features.Category.Requests.Queries;
using Fintracker.Application.Responses.API_Responses;
using Fintracker.Application.Responses.Commands_Responses;
using Fintracker.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[ApiController]
[Route("api/category")]
[Authorize(Roles = "Admin, User")]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
        
    }

    private Guid GetCurrentUserId()
    {
        var uid = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypeConstants.Uid)?.Value;
        if (Guid.TryParse(uid, out var currentUserId))
            return currentUserId;
        return Guid.Empty;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<CategoryDTO>>> Get([FromQuery] string? sortBy, [FromQuery] bool? isDescending)
    {
        var sortRequest = new GetCategoriesSortedRequest
        {
            IsDescending = isDescending.HasValue && isDescending.Value,
            SortBy = sortBy!,
            UserId = GetCurrentUserId()
        };

        var simpleRequest = new GetCategoriesRequest()
        {
            
            UserId = GetCurrentUserId()
        };

        IReadOnlyList<CategoryDTO> response;

        if (!string.IsNullOrEmpty(sortBy))
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

    [HttpGet("{type}")]
    [ProducesResponseType(typeof(List<CategoryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<CategoryDTO>>> Get(CategoryType type)
    {
        var response = await _mediator.Send(new GetCategoriesByTypeRequest
        {
            Type = type,
            UserId = GetCurrentUserId()
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
            Category = category
        });

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(DeleteCommandResponse<CategoryDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteCommandResponse<CategoryDTO>>> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteCategoryCommand
        {
            Id = id,
            UserId = GetCurrentUserId()
        });

        return Ok(response);
    }
}