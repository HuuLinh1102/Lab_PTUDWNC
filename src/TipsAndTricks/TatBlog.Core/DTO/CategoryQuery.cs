using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
	public class CategoryQuery
	{

		public string Keyword { get; set; }
		public string CategorySlug { get; set; }
		public bool ShowOnMenu { get; set; } = true;
		public bool NotShowOnMenu { get; set; } = false;

	}
}
