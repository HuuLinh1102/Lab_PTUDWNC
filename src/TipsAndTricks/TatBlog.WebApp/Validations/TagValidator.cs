using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
	public class TagValidator : AbstractValidator<TagEditModel>
	{
		public readonly IBlogRepository _blogRepository;

		public TagValidator(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;

			RuleFor(x => x.Name)
				.NotEmpty()
				.MaximumLength(500)
				.WithMessage("Tên không được để trống");

			RuleFor(x => x.UrlSlug)
				.NotEmpty()
				.MaximumLength(1000)
				.WithMessage("Tên định danh không được để trống");

			RuleFor(x => x.UrlSlug)
				.MustAsync(async (tagModel, slug, cancellationToken) =>
				!await blogRepository.IsTagSlugExistedAsync(
					tagModel.Id, slug))
				.WithMessage("Slug '{PropertyValue}' đã được sử dụng");

			RuleFor(x => x.Description)
				.NotEmpty()
				.WithMessage("Nội dung không được để trống");

		}
	}
}
