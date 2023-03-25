using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Runtime.CompilerServices;
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
            [FromQuery(Name = "k")] string keyword = null,
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
                .GetPagedPostsAsync(postQuery, pageNumber, pageSize);

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

        public async Task<IActionResult> Post(
            [FromRoute(Name = "year")] int year = 2022,
            [FromRoute(Name = "month")] int month = 9,
            [FromRoute(Name = "day")] int day = 5,
            [FromRoute(Name = "slug")] string slug = null)
        {
            var postQuery = new PostQuery()
            {
                Month = month,
                Year = year,
                Day = day,
                TitleSlug = slug,
            };
            var post = await _blogRepository.GetPostDetailAsync(postQuery);
            try
            {
                if (post != null && !post.Published)
                {
                    await _blogRepository.IncreaseViewCountAsync(post.Id);
                }
            }
            catch (NullReferenceException)
            {
                return View("Error");
            }
            var relatedPosts = await _blogRepository.GetRelatedPostsAsync(post.Id, post.Category.Id);
            ViewBag.relatedPosts = relatedPosts;
            return View("DetailPost", post);
        }

        public async Task<IActionResult> Archives(
            [FromRoute(Name = "year")] int year = 2020,
            [FromRoute(Name = "month")] int month = 2)
        {
            var postQuery = new PostQuery()
            {
                Month = month,
                Year = year,
            };
            ViewBag.PostQuery = postQuery;
            var posts = await _blogRepository.GetPagedPostsAsync(postQuery,1,10);
            return View("Archives", posts);

        }

		public async Task<IActionResult> PopularPosts(
			string slug,
			int pageNumber = 1,
			int pageSize = 10)
		{
			var post = await _blogRepository.FindBySlugAsync<Post>(slug);

			if (post == null)
			{
				return NotFound();
			}

			var postQuery = new PostQuery()
			{
				PublishedOnly = true,
				TitleSlug = post.UrlSlug
			};

			var posts = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, pageSize);
            ViewBag.Tag = posts;
			return View("DetailPost", posts.FirstOrDefault());
		}

		public async Task<IActionResult> PopularAuthors(
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

			var posts = await _blogRepository.GetPagedPostsAsync(postQuery, pageNumber, pageSize);
			ViewBag.Tag = posts;
			return View("Index",posts);
		}

		public IActionResult About()
            => View();

        public IActionResult Contact()
            => View();

        public IActionResult Rss()
            => Content("Nội dung sẽ được cập nhật");
    }
}
