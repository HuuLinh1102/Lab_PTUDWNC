using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
	public class AuthorValidator : AbstractValidator<AuthorEditModel>
	{
		public readonly IBlogRepository _blogRepository;

		public AuthorValidator(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;

			RuleFor(a => a.FullName)
				.NotEmpty()
				.WithMessage("Tên tác giả không được để trống")
				.MaximumLength(500)
				.WithMessage("Tên tác giả dài tối đa '{MaxLength}' ký tự");

			RuleFor(a => a.UrlSlug)
				.NotEmpty()
				.WithMessage("Slug không được để trống")
				.MaximumLength(1000)
				.WithMessage("Slug dài tối đa '{MaxLength}' ký tự");

			RuleFor(a => a.UrlSlug)
				.MustAsync(async (slug, cancellationToken) => !await _blogRepository
				.IsAuthorSlugExistedAsync(0, slug, cancellationToken))
				.WithMessage("Slug '{PropertyValue}' đã được sử dụng");

			RuleFor(a => a.Email)
				.NotEmpty()
				.WithMessage("Email của tác giả không được để trống");

			When(a => a.Id <= 0, () =>
			{
				RuleFor(a => a.ImageFile)
				.Must(f => f is { Length: > 0 })
				.WithMessage("Bạn phải chọn hình ảnh");
			})
			.Otherwise(() =>
			{
				RuleFor(a => a.ImageFile)
				.MustAsync(SetImageIfNotExist)
				.WithMessage("Bạn phải chọn hình ảnh");
			});
		}

		private async Task<bool> SetImageIfNotExist(
			AuthorEditModel auhorEditModel, 
			IFormFile imageFile, 
			CancellationToken cancellationToken)
		{
			var author = await _blogRepository.GetAuthorByIdAsync(auhorEditModel.Id);

			// Nếu đã có hình ảnh => Không bắt buộc chọn file
			if (!string.IsNullOrWhiteSpace(author?.ImageUrl))
				return true;

			// Ngược lại (chưa có hình ảnh), kiểm tra xem
			// người dùng đã chọn file hay chưa. Nếu chưa thì báo lỗi
			return imageFile is { Length: > 0 };
		}

	}
}

