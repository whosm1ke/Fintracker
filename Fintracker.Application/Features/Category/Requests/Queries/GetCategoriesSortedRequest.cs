﻿using Fintracker.Application.DTO.Category;
using MediatR;

namespace Fintracker.Application.Features.Category.Requests.Queries;

public class GetCategoriesSortedRequest : IRequest<CategoryDTO>
{
    public string SortBy { get; set; }
}