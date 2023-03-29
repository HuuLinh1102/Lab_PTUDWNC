using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Entities
{
	public class Comment
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool IsApproved { get; set; }
		public int PostId { get; set; }
		public Post Post { get; set; }
		public int AuthorId { get; set; }
		public Subscriber Author { get; set; }
	}
}
