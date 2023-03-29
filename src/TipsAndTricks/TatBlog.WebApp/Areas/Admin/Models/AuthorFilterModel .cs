using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class AuthorFilterModel
	{
		[DisplayName("Từ khóa")]
		public string Keyword { get; set; }

		[DisplayName("Slug")]
		public string UrlSlug { get; set; }

		[DisplayName("Email tác giả")]
		public string Email { get; set; }

	}
}
