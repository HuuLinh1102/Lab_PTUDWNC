using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
	public class CategoryValidator : AbstractValidator<CategoryEditModel>
	{
		public CategoryValidator() 
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.MaximumLength(500)
				.WithMessage("Tên không được để trống");

			RuleFor(x => x.UrlSlug)
				.NotEmpty()
				.MaximumLength(1000)
				.WithMessage("Tên định danh không được để trống");

			RuleFor(x => x.Description)
				.NotEmpty()
				.WithMessage("Nội dung không được để trống");
		}
	}
}
