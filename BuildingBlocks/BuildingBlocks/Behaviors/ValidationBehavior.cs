﻿using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>


    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var ValidationResults = await Task.WhenAll(validators.Select(x => x.ValidateAsync(context, cancellationToken)));

            var Failures = ValidationResults.Where(r => r.Errors.Any()).SelectMany(r => r.Errors).ToList();
            if (Failures.Any())
            {
                throw new ValidationException(Failures);
            }
            return await next();
        }
    }
}
