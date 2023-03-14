using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.DTO
{
    public class PostQuery
    {
        // cho phép null
        public int? AuthorId { get; set; }
        public string AuthorSlug { get; set; }
        public int? CategoryId { get; set; }
<<<<<<< HEAD
        public int? TagId { get; set; }
        public string CategorySlug { get; set; }
		public string TagSlug { get; set; }
		public string TitleSlug { get; set; }
		public int? Year { get; set; }
        public int? Month { get; set; }
        public bool? PublishedOnly { get; set; }
        public bool? NotPublished { get; set; }
=======
        public string CategorySlug { get; set; }
		public string TagSlug { get; set; }
		public int? Year { get; set; }
        public int? Month { get; set; }
        public bool? PublishedOnly { get; set; }
>>>>>>> 8f78ca59d326612ec5d6d800c3a2375fe0af6af1
        public string Keyword { get; set; }
    }
}
