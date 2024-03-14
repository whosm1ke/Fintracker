using Fintracker.Application.Contracts.Helpers;
using Fintracker.Application.Exceptions;
using FluentValidation;
using MediatR.Pipeline;

namespace Fintracker.Application.Features.Behaviours;

public class ValidationBehaviourPreProcess<TRequest> : IRequestPreProcessor<TRequest> where TRequest : INotGetRequest
{
    private readonly IValidator<TRequest> _validator;
    public ValidationBehaviourPreProcess(IValidator<TRequest> validator)
    {
        _validator = validator;
    }
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (_validator is not null)
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validator.ValidateAsync(context, cancellationToken));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
            if (failures.Count != 0)
                throw new BadRequestException(failures.Select(x => new ExceptionDetails
                {
                    ErrorMessage = x.ErrorMessage,
                    PropertyName = x.PropertyName
                }).ToList());
        }
    }
}