using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class CategoryEditModel
	{
		public int Id { get; set; }

		[DisplayName("Tên chủ đề")]
		[Required(ErrorMessage = "Vui lòng nhập tên chủ đề")]
		public string Name { get; set; }

		[DisplayName("Slug")]
		[Required(ErrorMessage = "Vui lòng nhập slug")]
		[Remote("VerifyCategorySlug", "Categories", "Admin",
			HttpMethod = "POST", AdditionalFields = "Id")]
		public string UrlSlug { get; set; }


		[DisplayName("Mô tả")]
		public string Description { get; set; }


		[DisplayName("Hiển thị trên menu")]
		public bool ShowOnMenu { get; set; }


	}
}
