﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class TagEditModel
	{
		public int Id { get; set; }

		[DisplayName("Tên thẻ")]
		[Required(ErrorMessage = "Vui lòng nhập tên thẻ")]
		public string Name { get; set; }

		[DisplayName("Slug")]
		[Required(ErrorMessage = "Vui lòng nhập slug")]
		[Remote("VerifyTagSlug", "Tags", "Admin",
			HttpMethod = "POST", AdditionalFields = "Id")]
		public string UrlSlug { get; set; }

		[DisplayName("Mô tả")]
		public string Description { get; set; }

	}
}
