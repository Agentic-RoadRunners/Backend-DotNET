// This file implements a MediatR pipeline behavior for validating requests using FluentValidation. It checks if there are any validators for the incoming request, and if so, it executes them in parallel. If any validation errors occur, it throws a BadRequestException with the error messages. If validation is successful, it allows the request to proceed to the next behavior or handler in the pipeline.
using FluentValidation;
using MediatR;
using SafeRoad.Core.Exceptions;

namespace SafeRoad.Core.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators; // injection of validator of request

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next(); // if there is no validator, continue to the next behavior or handler

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));// parallel validation of all validators
        var failures = results.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Any())
            throw new BadRequestException(failures.Select(f => f.ErrorMessage).ToList());// if there are validation errors, throw a BadRequestException with the error messages

        return await next();// if validation is successful, continue to the next behavior or handler
    }
}