using Fintracker.Application.DTO.Monobank;
using MediatR;

namespace Fintracker.Application.Features.Monobank.Requests.Queries;

public class GetMonobankUserInfoRequest : IRequest<MonobankUserInfoDTO>
{
    /// <summary>
    /// Should be provided from header
    /// </summary>
    public string Token { get; set; } = "0";
}