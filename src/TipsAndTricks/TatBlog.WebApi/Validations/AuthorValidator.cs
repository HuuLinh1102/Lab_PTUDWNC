using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations
{
	public class AuthorValidator: AbstractValidator<AuthorEditModel>
	{
		public AuthorValidator() 
		{
			RuleFor(a => a.FullName)
				.NotEmpty()
				.WithMessage("Tên tác giả không được để trống")
				.MaximumLength(100)
				.WithMessage("Tên tác giả dài tối đa 100 ký tự");

			RuleFor(a => a.UrlSlug)
				.NotEmpty()
				.WithMessage("Slug không được để trống")
				.MaximumLength(1000)
				.WithMessage("Slug dài tối đa 100 ký tự");

			RuleFor(a => a.JoinedDate)
				.GreaterThan(DateTime.MinValue)
				.WithMessage("Ngày tham gia không hợp lệ");

			RuleFor(a => a.Email)
				.NotEmpty()
				.WithMessage("Email không được để trống")
				.MaximumLength(100)
				.WithMessage("Email chứa tối đa 100 ký tự");

			RuleFor(a => a.Notes)
				.MaximumLength(500)
				.WithMessage("Ghi chú tối đa 500 ký tự");
		}
	}
}
