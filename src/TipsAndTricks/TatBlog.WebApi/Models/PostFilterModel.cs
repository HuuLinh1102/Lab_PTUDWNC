using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApi.Models
{
	public class PostFilterModel
	{

		public string Keyword { get; set; }

		public int? AuthorId { get; set; }

		public int? CategoryId { get; set; }

		public int? Year { get; set; }

		public int? Month { get; set; }

		public IEnumerable<SelectListItem> AuthorList { get; set; }

		public IEnumerable<SelectListItem> CategoryList { get; set; }

		public IEnumerable<SelectListItem> MonthList { get; set; }

		public PostFilterModel()
		{
			MonthList = Enumerable.Range(1, 12)
				.Select(m => new SelectListItem()
				{
					Value = m.ToString(),
					Text = CultureInfo.CurrentCulture
						 .DateTimeFormat.GetMonthName(m)
				}).ToList();
		}

	}
}
