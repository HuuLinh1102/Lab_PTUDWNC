using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository) 
        {
            _blogRepository = blogRepository;
        }


        // Action này xử lý HTTP request đến trang chủ của
        // ứng dụng web hoặc tìm kiếm bài viết theo từ khóa
        public async Task<IActionResult> Index(
            [FromQuery(Name ="k")] string keyword = null,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10,
			string slug = null) 
        {
            // Tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                // Chỉ lấy những bài viết có trạng thái Published
                PublishedOnly = true,
                // Tìm bài viết theo từ khóa
                Keyword = keyword,
                
              
            };

            // Truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository
                .GetPagedPostsAsync(postQuery,pageNumber,pageSize);

            // Lưu lại điều kiện truy vấn để hiện thị trong view
            ViewBag.PostQuery = postQuery;

            // Truyền ds bài viết vào view để render ra html
            return View(postsList);
        
        }

        // Hiển thị bài viết theo chủ đề
        public async Task<IActionResult> Category(
			string slug, 
            int pageNumber = 1, 
            int pageSize = 10)
        {
			// Lấy thông tin chủ đề từ slug
			var category = await _blogRepository.FindBySlugAsync<Category>(slug);

			if (category == null)
			{
				// Nếu không tìm thấy chủ đề thì trả về trang 404
				return NotFound();
			}

			// Tạo đối tượng chứa các điều kiện truy vấn
			var postQuery = new PostQuery()
			{
				// Chỉ lấy những bài viết có trạng thái Published
				PublishedOnly = true,
				// Lọc bài viết theo chủ đề
				CategorySlug = category.UrlSlug
			};

			// Truy vấn các bài viết theo điều kiện đã tạo
			var postsList = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, pageSize);

			// Lưu lại thông tin chủ đề để hiển thị trong view
			ViewBag.Category = category;

			// Truyền danh sách bài viết vào view để render ra html
			return View(postsList);
		}

		// Hiển thị bài viết theo tác giả
		public async Task<IActionResult> Author(
			string slug,
			int pageNumber = 1,
			int pageSize = 10)
		{
			var author = await _blogRepository.FindBySlugAsync<Author>(slug);

			if (author == null)
			{
				return NotFound();
			}

			var postQuery = new PostQuery()
			{
				PublishedOnly = true,
				AuthorSlug = author.UrlSlug
			};

			var postsList = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, pageSize);
			ViewBag.Author = author;
			return View(postsList);
		}

		// Hiển thị bài viết theo tag
		public async Task<IActionResult> Tag(
			string slug,
			int pageNumber = 1,
			int pageSize = 10)
		{
			var tag = await _blogRepository.FindBySlugAsync<Tag>(slug);

			if (tag == null)
			{
				return NotFound();
			}

			var postQuery = new PostQuery()
			{
				PublishedOnly = true,
				TagSlug = tag.UrlSlug
			};

			var postsList = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, pageSize);
			ViewBag.Tag = tag;
			return View(postsList);
		}

		public IActionResult About()
            => View();

        public IActionResult Contact()
            => View();

        public IActionResult Rss()
            => Content("Nội dung sẽ được cập nhật");
    }
}
