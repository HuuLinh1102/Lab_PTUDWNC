using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations
{
	public class PostValidator : AbstractValidator<PostEditModel>
	{
		public readonly IBlogRepository _blogRepository;

		public PostValidator(IBlogRepository blogRepository) 
		{
			_blogRepository = blogRepository;

			RuleFor(x => x.Title)
				.NotEmpty()
				.MaximumLength(500)
				.WithMessage("Tiêu đề không được để trống");


			RuleFor(x => x.ShortDescription)
				.NotEmpty()
				.WithMessage("Nội dung tóm tắt không được để trống");

			RuleFor(x => x.Description)
				.NotEmpty()
				.WithMessage("Nội dung không được để trống");

			RuleFor(x => x.Meta)
				.NotEmpty()
				.MaximumLength(1000)
				.WithMessage("Meta không được để trống");

			RuleFor(x => x.UrlSlug)
				.NotEmpty()
				.MaximumLength(1000)
				.WithMessage("Tên định danh không được để trống");

			RuleFor(x => x.UrlSlug)
				.MustAsync(async (postModel, slug, cancellationToken) =>
				!await blogRepository.IsPostSlugExistedAsync(
					postModel.Id, slug, cancellationToken))
				.WithMessage("Slug '{PropertyValue}' đã được sử dụng");

			RuleFor(x => x.CategoryId)
				.NotEmpty()
				.WithMessage("Bạn phải chọn chủ đề cho bài viết");

			RuleFor(x => x.AuthorId)
				.NotEmpty()
				.WithMessage("Bạn phải chọn tác giả của bài viết");

			RuleFor(x => x.SelectedTags)
				.Must(HasAtLeastOneTag)
				.WithMessage("Bạn phải chọn ít nhất một thẻ");

			When(x => x.Id <= 0, () =>
			{
				RuleFor(x => x.ImageFile)
				.Must(x => x is { Length: > 0 })
				.WithMessage("Bạn phải chọn hình ảnh cho bài viết");
			})
			.Otherwise(() =>
			{
				RuleFor(x => x.ImageFile)
				.MustAsync(SetImageIfNotExist)
				.WithMessage("Bạn phải chọn hình ảnh cho bài viết");
			});
		}

		// KT ng dùng đã nhập ít nhất 1 tag
		private bool HasAtLeastOneTag(
			PostEditModel postModel, string selectedTags)
		{
			return postModel.GetSelectedTag().Any();
		}

		// KT hình ảnh
		private async Task<bool> SetImageIfNotExist(
			PostEditModel postModel,
			IFormFile imageFile,
			CancellationToken cancellationToken)
		{
			var post = await _blogRepository.GetPostByIdAsync(
				postModel.Id, false, cancellationToken);

			if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
				return true;

			return imageFile is { Length: > 0 };
		}
	}
}
