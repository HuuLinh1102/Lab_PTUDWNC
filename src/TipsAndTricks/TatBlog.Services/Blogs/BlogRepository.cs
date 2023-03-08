using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extentions;

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {

        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context)
        {
            _context = context;
        }



        // Tìm bài viết có tên định danh là "slug"
        // và được đăng vào tháng 'month' năm year'
        public async Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsQuery = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author);

            if (year > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
            }
            if (month > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
            }
            if (!string.IsNullOrWhiteSpace(slug))
            {
                postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
            }

            return await postsQuery.FirstOrDefaultAsync(cancellationToken);
        }


        // Tìm Top N bài viết phổ được nhiều người xem nhất 
        public async Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        // Kiểm tra xem tên định danh của bài viết đã có hay chưa 
        public async Task<bool> IsPostSlugExistedAsync(
            int postId, string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug,
                cancellationToken);
        }


        // Tăng số lượt xem của một bài viết
        public async Task IncreaseViewCountAsync(
            int postId, CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p =>
                p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1),
                cancellationToken);
        }

        public async Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();

            if (showOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu);
            }
            return await categories
               .OrderBy(x => x.Name)
               .Select(x => new CategoryItem()
               {
                   Id = x.Id,

                   Name = x.Name,

                   UrlSlug = x.UrlSlug,

                   Description = x.Description,

                   ShowOnMenu = x.ShowOnMenu,

                   PostCount = x.Posts.Count(p => p.Published)
               })
               .ToListAsync(cancellationToken);
        }

        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default)

        {
            var tagQuery = _context.Set<Tag>().Select(x => new TagItem()

            {

                Id = x.Id,
                Name = x.Name,
                UrlSlug = x.UrlSlug,
                Description = x.Description,
                PostCount = x.Posts.Count(p => p.Published)
            });

            return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }


        // Tìm một thẻ, chuyên mục, bài viết theo tên định danh (slug) 
        public async Task<T> FindBySlugAsync<T>(string slug) where T : class, IEntity
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.UrlSlug == slug);
        }

        // Tìm một thẻ, chuyên mục, bài viết theo id
        public async Task<T> FindByIdAsync<T>(int id) where T : class, IEntity
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }

        // Lấy danh sách tất cả các thẻ
        public async Task<IList<TagItem>> GetTagsAsync(
            CancellationToken cancellationToken = default)
        {
            IQueryable<Tag> tags = _context.Set<Tag>();

            return await tags
               .OrderBy(t => t.Name)
               .Select(t => new TagItem()
               {
                   Id = t.Id,

                   Name = t.Name,

                   UrlSlug = t.UrlSlug,

                   Description = t.Description,

                   PostCount = t.Posts.Count(p => p.Published)
               })
               .ToListAsync(cancellationToken);
        }

        // Xóa một thẻ,danh mục theo mã cho trước. 
        public async Task DeleteByIdAsync<T>(int id) where T : class, new()
        {
            // tìm thẻ có id đó
            var entity = await _context.Set<T>().FindAsync(id);

            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine($"Tag with id {id} not found.");
            }
        }


        // Thêm hoặc cập nhật một chuyên mục/chủ đề.
        public async Task<Category> AddOrUpdateCategoryAsync(Category category)
        {
            if (category == null)
            {
                Console.WriteLine("No category value is passed in.");
            }
            if (category.Id == 0)
            {
                // thêm mới danh mục
                category.Name = category.Name;
                category.UrlSlug = GenerateSlug(category.Name);
                category.Description = category.Description;
                await _context.Categories.AddAsync(category);
                Console.WriteLine("Successfully added category.");
            }
            else
            {
                // cập nhập danh mục 
                var existingCategory = await _context.Categories.FindAsync(category.Id);

                if (existingCategory == null)
                {
                    Console.WriteLine($"Category with id {category.Id} not found.");
                }

                existingCategory.Name = category.Name;
                existingCategory.UrlSlug = GenerateSlug(category.Name);
                existingCategory.Description = category.Description;
                Console.WriteLine("Successfully updated category.");
            }

            await _context.SaveChangesAsync();

            return category;
        }
        // Tạo tên định danh (slug)
        private static string GenerateSlug(string phrase)
        {
            var str = phrase.ToLowerInvariant().Trim();

            // xóa ký tự không hợp lệ
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            // chuyển nhiều khoảng trắng thành 1
            str = Regex.Replace(str, @"\s+", " ").Trim();

            // cắt và lấy tối đa 50 ký tự, xóa khoảng trắng đầu và cuối
            str = str.Substring(0, str.Length <= 50 ? str.Length : 50).Trim();

            // thay khoảng trắng bằng gạch ngang "-"
            str = Regex.Replace(str, @"\s", "-");

            return str;
        }


        // Kiểm tra tên định danh (slug) của một chuyên mục đã tồn tại hay chưa.
        public async Task<bool> IsSlugExists(string slug)
        {
            var categories = await GetCategoriesAsync();

            return categories.Any(c => c.UrlSlug.Equals(slug, 
                StringComparison.OrdinalIgnoreCase)); // không phân biệt hoa thường
        }

        // Lấy và phân trang danh sách chuyên mục
        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default)

        {
            var categoryQuery = _context.Set<Category>().Select(c => new CategoryItem()

            {

                Id = c.Id,
                Name = c.Name,
                UrlSlug = c.UrlSlug,
                Description = c.Description,
                ShowOnMenu = c.ShowOnMenu,
                PostCount = c.Posts.Count(p => p.Published)
            });

            return await categoryQuery.ToPagedListAsync(pagingParams, cancellationToken);
        }

        // Đếm số lượng bài viết trong N tháng gần nhất
        public async Task<IList<MonthlyPostCount>> GetMonthlyPostCountsAsync(int months)
        {
            var now = DateTime.UtcNow;
            var posts = await _context.Posts.ToListAsync();

            var monthlyCounts = new List<MonthlyPostCount>();
            for (int i = 0; i < months; i++)
            {
                var month = now.AddMonths(-i);
                monthlyCounts.Add(new MonthlyPostCount { Year = month.Year, Month = month.Month, Count = 0 });
            }

            foreach (var post in posts)
            {
                if (post.PostedDate != null && post.PostedDate >= now.AddMonths(-months))
                {
                    var year = post.PostedDate.Year;
                    var month = post.PostedDate.Month;
                    var count = monthlyCounts.FirstOrDefault(m => m.Year == year && m.Month == month);
                    if (count != null)
                    {
                        count.Count++;
                    }
                }
            }

            return monthlyCounts;
        }

        // Thêm hay cập nhật một bài viết. 
        public async Task<Post> AddOrUpdatePostAsync(Post post)
        {
            if (post == null)
            {
                Console.WriteLine("No category value is passed in.");
            }
            if (post.Id == 0)
            {
                // thêm mới danh mục
                //post.Title = post.Title;
                //post.ShortDescription = post.ShortDescription;
                //post.Description = post.Description;
                //post.Meta = post.Meta;
                post.UrlSlug = GenerateSlug(post.Title);
                //post.Published = post.Published;
                //post.PostedDate = post.PostedDate;
                //post.ModifiedDate = post.ModifiedDate;
                //post.ViewCount = post.ViewCount;
                //post.Author = post.Author;
                //post.Category = post.Category;
                //post.Tags = post.Tags;
                post.PostedDate = DateTime.Now;

                await _context.Posts.AddAsync(post);
                Console.WriteLine("Successfully added post");
            }
            else
            {
                // cập nhập bài viết
                var existingPost = await _context.Posts.FindAsync(post.Id);

                if (existingPost == null)
                {
                    Console.WriteLine($"Post with id {post.Id} not found.");
                }
                existingPost.Title = post.Title;
                existingPost.ShortDescription = post.ShortDescription;
                existingPost.Description = post.Description;
                existingPost.Meta = post.Meta;
                existingPost.UrlSlug = GenerateSlug(post.Title);
                existingPost.Published = post.Published;
                existingPost.ModifiedDate = post.Published ? DateTime.Now : (DateTime?)null; // kiểm tra xewm nó được xuất bản chưa nếu r thì cập nhật thời gian
                existingPost.Category = post.Category;
                existingPost.Tags = post.Tags;
                Console.WriteLine("Successfully updated p");
            }

            await _context.SaveChangesAsync();

            return post;
        }

        // Chuyển đổi trạng thái Published của bài viết. 
        public async Task<bool> ChangePostPublishedStatus(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                Console.WriteLine($"Post with id {id} not found.");
            }

            post.Published = !post.Published;

            if (post.Published)
            {
                post.PostedDate = DateTime.Now;
            }

            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return post.Published;
        }

		// Tìm và phân trang các bài viết thỏa mãn điều kiện tìm kiếm 

		public async Task<IPagedList<Post>> GetPagedPostsAsync(
            PostQuery query,
			int pageNumber = 1,
			int pageSize = 10,
			CancellationToken cancellationToken = default)
		{
			var posts = _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.Tags)
                .Where(p => p.Published);

			// Thực hiện các bộ lọc tìm kiếm trên đối tượng query
			if (!string.IsNullOrEmpty(query.Keyword))
			{
				posts = posts.Where(p => p.Title.Contains(query.Keyword));
			}
			if (query.AuthorId.HasValue)
			{
				posts = posts.Where(p => p.Author.Id == query.AuthorId.Value);
			}
			if (query.CategoryId.HasValue)
			{
				posts = posts.Where(p => p.Category.Id == query.CategoryId.Value);
			}

			if (!string.IsNullOrEmpty(query.CategorySlug))
			{
				posts = posts.Where(p => p.UrlSlug.Contains(query.CategorySlug));
			}
			if (query.Year != null)
			{
				posts = posts.Where(p => p.PostedDate.Year == query.Year);
			}

			if (query.Month != null)
			{
				posts = posts.Where(p => p.PostedDate.Month == query.Month);
			}

			// Phân trang các bài post bằng thư viện PagedList
			return await posts.ToPagedListAsync(
				pageNumber, pageSize,
				nameof(Post.PostedDate), "DESC",
				cancellationToken);
		}

        // t.
		public async Task<IPagedList<T>> GetPagedTAsync<T>(PostQuery query, 
            Func<IQueryable<Post>, 
                IQueryable<T>> mapper, 
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
			var posts = _context.Posts
				.Include(p => p.Author)
				.Include(p => p.Category)
				.Where(p => p.Published);

			// Thực hiện các bộ lọc tìm kiếm trên đối tượng query
			if (!string.IsNullOrEmpty(query.Keyword))
            {
                posts = posts.Where(p => p.Title.Contains(query.Keyword));
            }
            if (query.AuthorId.HasValue)
            {
                posts = posts.Where(p => p.Author.Id == query.AuthorId.Value);
            }
            if (query.CategoryId.HasValue)
            {
                posts = posts.Where(p => p.Category.Id == query.CategoryId.Value);
            }

            if (!string.IsNullOrEmpty(query.CategorySlug))
            {
                posts = posts.Where(p => p.UrlSlug.Contains(query.CategorySlug));
            }
            if (query.Year != null)
            {
                posts = posts.Where(p => p.PostedDate.Year == query.Year);
            }

            if (query.Month != null)
            {
                posts = posts.Where(p => p.PostedDate.Month == query.Month);
            }

            // Ánh xạ các đối tượng Post thành các đối tượng T bằng mapper
            var items = mapper(posts);
            // Phân trang các đối tượng T bằng thư viện PagedList
            return await items.ToPagedListAsync(
                pageNumber, pageSize, 
                nameof(Post.PostedDate), "DESC", 
                cancellationToken);
        }



    }
}
