using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Filters
{
	public class ValidatorFilter<T> : IEndpointFilter where T : class
	{
		private readonly IValidator<T> _validator;
		private readonly ILogger<ValidatorFilter<T>> _logger;

		public ValidatorFilter(IValidator<T> validator, 
			ILogger<ValidatorFilter<T>> logger)
		{
			_validator = validator;
			_logger = logger;
		}

		public async ValueTask<object> InvokeAsync(
			EndpointFilterInvocationContext context,
			EndpointFilterDelegate next)
		{
			var model = context.Arguments
				.SingleOrDefault(x => x?.GetType() == typeof(T)) as T;

			if (model == null) 
			{
				_logger.LogError("Could not create model object");
				return Results.BadRequest(
					new ValidationFailureResponse(new[]
					{
						"Could not create model object"
					}));
			}

			var validationResult = await _validator.ValidateAsync(model);

			if(!validationResult.IsValid)
			{
				_logger.LogError(validationResult.Errors.ToResponse().ToString());
				return Results.BadRequest(
					validationResult.Errors.ToResponse());
			}

			return await next(context);
		}	

	}
}
