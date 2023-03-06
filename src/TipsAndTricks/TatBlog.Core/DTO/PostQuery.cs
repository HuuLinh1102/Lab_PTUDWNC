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
        public int? CategoryId { get; set; }
        public string CategorySlug { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public string Keyword { get; set; }
    }
}
