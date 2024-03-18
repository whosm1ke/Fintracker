using AutoMapper;
using Fintracker.Application.Contracts.Persistence;
using Fintracker.Application.Features.Category.Requests.Commands;
using MediatR;
using Newtonsoft.Json;

namespace Fintracker.Application.Features.Category.Handlers.Commands;

public class PopulateUserWithCategoriesCommandHandler : IRequestHandler<PopulateUserWithCategoriesCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PopulateUserWithCategoriesCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(PopulateUserWithCategoriesCommand request, CancellationToken cancellationToken)
    {
        string categoriesJson = await File.ReadAllTextAsync(request.PathToFile, cancellationToken);
        var categories = JsonConvert.DeserializeObject<IEnumerable<Domain.Entities.Category>>(categoriesJson);
        foreach (var category in categories!)
        {
            category.UserId = request.UserId;
            await _unitOfWork.CategoryRepository.AddAsync(category);
        }

        await _unitOfWork.SaveAsync();
        return Unit.Value;
    }
}