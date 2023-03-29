using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class CategoryFilterModel
	{
		[DisplayName("Từ khóa")]
		public string Keyword { get; set; }

		[DisplayName("Slug")]
		public string UrlSlug { get; set; }

		[DisplayName("Mô tả")]
		public string Description { get; set; }

		[DisplayName("Hiển thị trên menu")]
		public bool? ShowOnMenu { get; set; }

	}
}
