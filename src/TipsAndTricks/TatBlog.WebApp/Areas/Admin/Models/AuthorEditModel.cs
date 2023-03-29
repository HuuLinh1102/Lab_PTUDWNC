using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class AuthorEditModel
	{
		public int Id { get; set; }

		[DisplayName("Tên tác giả")]
		[Required(ErrorMessage = "Vui lòng nhập tên tác giả")]
		public string FullName { get; set; }

		[DisplayName("Slug")]
		[Required(ErrorMessage = "Vui lòng nhập slug")]
		[Remote("VerifyAuthorSlug", "Authors", "Admin",
			HttpMethod = "POST", AdditionalFields = "Id")]
		public string UrlSlug { get; set; }


		[DisplayName("Email")]
		[Required(ErrorMessage = "Vui lòng nhập email")]
		public string Email { get; set; }


		[DisplayName("Chọn ảnh")]
		public IFormFile ImageFile { get; set; }
		[Required(ErrorMessage = "Vui lòng chọn ảnh")]

		[DisplayName("Hình hiện tại")]
		public string ImageUrl { get; set; }

		

		[DisplayName("Ghi chú")]
		public string Notes { get; set; }
	}
}
