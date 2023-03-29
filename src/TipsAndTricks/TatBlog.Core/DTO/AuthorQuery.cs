using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
    public class AuthorQuery
    {
		public string Keyword { get; set; }
		public string AuthorSlug { get; set; }
		public string AuthorEmail { get; set; }
	}
}
