using Fintracker.Application;
using Fintracker.Application.DTO.User;
using Fintracker.Application.Features.User.Requests.Commands;
using Fintracker.Application.Features.User.Requests.Queries;
using Fintracker.Application.Responses.API_Responses;
using Fintracker.Application.Responses.Commands_Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Fintracker.API.Controllers;


[Route("api/user")]
[Authorize(Roles = "Admin,User")]
public class UserController : BaseController
{
    private readonly IMediator _mediator;
    private readonly AppSettings _appSettings;

    public UserController(IMediator mediator, IOptions<AppSettings> appSettings)
    {
        _mediator = mediator;
        _appSettings = appSettings.Value;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserBaseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserBaseDTO>> Get(Guid id)
    {
        var response = await _mediator.Send(new GetUserByIdRequest
        {
            Id = id
        });

        return Ok(response);
    }


    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(DeleteCommandResponse<UserBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DeleteCommandResponse<UserBaseDTO>>> Delete(Guid id)
    {
        var response = await _mediator.Send(new DeleteUserCommand
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(typeof(UpdateCommandResponse<UserBaseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(UnauthorizedResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(NotFoundResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UpdateCommandResponse<UserBaseDTO>>> Put([FromForm] UpdateUserDTO user,
        [FromServices] IWebHostEnvironment env)
    {
        if (user.Avatar != null)
        {
            var avatar = user.Avatar;
            var fileName = Path.GetFileName(avatar.FileName);
            var filePath = Path.Combine(env.WebRootPath, "images", fileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await avatar.CopyToAsync(stream);
            }

            user.UserDetails.Avatar = $"{_appSettings.BaseUrl}/api/user/avatar/{fileName}";
        }

        var response = await _mediator.Send(new UpdateUserCommand
        {
            User = user,
            WWWRoot = env.WebRootPath
        });

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("avatar/{filename}")]
    public IActionResult GetAvatar(string fileName, [FromServices] IWebHostEnvironment env)
    {
        var imageDirectory = Path.Combine(env.WebRootPath, "images");
        var imagePath = Path.Combine(imageDirectory, fileName);

        if (!System.IO.File.Exists(imagePath))
        {
            return NotFound();
        }

        var imageBytes = System.IO.File.ReadAllBytes(imagePath);
        var contentType = GetContentType(fileName);
        return File(imageBytes, contentType);
    }
    
    [NonAction]
    private string GetContentType(string filename)
    {
        var extension = Path.GetExtension(filename).ToLowerInvariant();
        switch (extension)
        {
            case ".png":
                return "image/png";
            case ".jpg":
            case ".jpeg":
                return "image/jpeg";
            case ".gif":
                return "image/gif";
            default:
                return "application/octet-stream";
        }
    }
}