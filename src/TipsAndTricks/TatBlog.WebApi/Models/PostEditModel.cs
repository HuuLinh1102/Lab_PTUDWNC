
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TatBlog.WebApi.Models
{
    public class PostEditModel
    {
		public int Id { get; set; }
		public string Title { get; set; }

		public string ShortDescription { get; set; }

		public string Description { get; set; }

        public string Meta { get; set; }

		public string UrlSlug { get; set; }

        public IFormFile ImageFile { get; set; }

        public string ImageUrl { get; set; }

        public bool Published { get; set; }

        public int CategoryId { get; set; }

        public int AuthorId { get; set; }

        public string SelectedTags { get; set; }


        public IEnumerable<SelectListItem> AuthorList { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }

        // Tách chuỗi chứa các thẻ thành 1 mảng
        public List<string> GetSelectedTag()
        {
            return (SelectedTags ?? "")
                .Split(new[] {',', ';', '\r', '\n' },
                    StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }
    }
}
