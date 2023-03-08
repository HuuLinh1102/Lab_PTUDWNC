using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
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
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10) 
        {
            // Tạo đối tượng chứa các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                // Chỉ lấy những bài viết có trạng thái Published
                PublishedOnly = true
            };

            // Truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository
                .GetPagedPostsAsync(postQuery,pageNumber,pageSize);

            // Lưu lại điều kiện truy vấn để hiện thị trong view
            ViewBag.PostQuery = postQuery;

            // Truyền ds bài viết vào view để render ra html
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
