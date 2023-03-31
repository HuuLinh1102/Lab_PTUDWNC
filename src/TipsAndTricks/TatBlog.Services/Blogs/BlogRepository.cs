using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

using System.Text.RegularExpressions;

using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Mappings;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {

        private readonly BlogDbContext _context;
		private readonly IMemoryCache _memoryCache;

		public BlogRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
			_memoryCache = memoryCache;
        }



        // Tìm bài viết có tên định danh là "slug"
        // và được đăng vào tháng 'month' năm year'
        public async Task<IList<Post>> GetPostsAsync(
        PostQuery condition,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition)
                .OrderByDescending(x => x.PostedDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken: cancellationToken);
        }
        public async Task<int> CountPostsAsync(
        PostQuery condition, CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition).CountAsync(cancellationToken: cancellationToken);
        }

        public async Task<Post> GetPostAsync(
		string slug,
		CancellationToken cancellationToken = default)
	    {
            var postQuery = new PostQuery()
            {
                PublishedOnly = true,
                TitleSlug = slug
            };

            return await FilterPosts(postQuery).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Post> GetPostDetailAsync(
        PostQuery condition,
        CancellationToken cancellationToken = default)
        {
            return await FilterPosts(condition).FirstOrDefaultAsync(cancellationToken);
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




		public async Task<IList<Post>> GetRandomArticlesAsync(
		int numPosts, CancellationToken cancellationToken = default)
		{
			return await _context.Set<Post>()
				.OrderBy(x => Guid.NewGuid())
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


        public async Task<Post> GetPostByIdAsync(
        int postId, bool includeDetails = false,
        CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await _context.Set<Post>().FindAsync(postId);
            }

            return await _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == postId, cancellationToken);
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

		public async Task<Tag> GetTagAsync(
		string slug, CancellationToken cancellationToken = default)
		{
			return await _context.Set<Tag>()
				.FirstOrDefaultAsync(x => x.UrlSlug == slug, cancellationToken);
		}

		// Lấy danh sách tất cả các thẻ
		public async Task<IList<TagItem>> GetTagsAsync(
		 CancellationToken cancellationToken = default)
		{
			return await _context.Set<Tag>()
				.OrderBy(x => x.Name)
				.Select(x => new TagItem()
				{
					Id = x.Id,
					Name = x.Name,
					UrlSlug = x.UrlSlug,
					Description = x.Description,
					PostCount = x.Posts.Count(p => p.Published)
				})
				.ToListAsync(cancellationToken);
		}

		public async Task<bool> CreateOrUpdateTagAsync(
		Tag tag, CancellationToken cancellationToken = default)
		{
			if (tag.Id > 0)
			{
				_context.Set<Tag>().Update(tag);
			}
			else
			{
				_context.Set<Tag>().Add(tag);
			}

			return await _context.SaveChangesAsync(cancellationToken) > 0;
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

		public async Task<bool> DeletePostAsync(
		int postId, CancellationToken cancellationToken = default)
		{
			return await _context.Posts
				.Where(x => x.Id == postId)
				.ExecuteDeleteAsync(cancellationToken) > 0;
		}

		// Thêm hoặc cập nhật một bài viết.
		public async Task<Post> CreateOrUpdatePostAsync(
		Post post, IEnumerable<string> tags,
		CancellationToken cancellationToken = default)
		{
			if (post.Id > 0)
			{
				await _context.Entry(post)
							  .Collection(x => x.Tags)
							  .LoadAsync(cancellationToken);
			}
			else
			{
				post.Tags = new List<Tag>();
			}

            var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => new
                {
                    Name = x,
                    Slug = GenerateSlug(x)
                })
				.GroupBy(x => x.Slug)
				.ToDictionary(g => g.Key, g => g.First().Name);

			foreach (var kv in validTags)
			{
				if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, 
					StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

				var tag = await GetTagAsync(kv.Key, cancellationToken) ?? new Tag()
				{
					Name = kv.Value,
					Description = kv.Value,
					UrlSlug = kv.Key
				};

				post.Tags.Add(tag);
			}

			post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

			if (post.Id > 0)
				_context.Update(post);
			else
				_context.Add(post);

			await _context.SaveChangesAsync(cancellationToken);

			return post;
		}


		public async Task<bool> AddOrUpdateAsync(
			Post post, 
			IEnumerable<string> tags, 
			CancellationToken cancellationToken = default)
		{
			if (post.Id > 0)
			{
				// Xóa các tag của bài viết cũ trong database
				var oldPost = await _context.Posts.Include(p => p.Tags)
					.FirstOrDefaultAsync(p => p.Id == post.Id, cancellationToken);
				oldPost.Tags.Clear();
				await _context.SaveChangesAsync(cancellationToken);

				// Thêm các tag mới cho bài viết
				foreach (var tagName in tags)
				{
					var tag = await _context.Tags
						.FirstOrDefaultAsync(t => t.Name == tagName, cancellationToken);
					if (tag == null)
					{
						tag = new Tag { Name = tagName };
						_context.Tags.Add(tag);
					}

					post.Tags.Add(tag);
				}

				_context.Posts.Update(post);
				_memoryCache.Remove($"post.by-id.{post.Id}");
			}
			else
			{
				// Thêm các tag mới cho bài viết
				foreach (var tagName in tags)
				{
					var tag = await _context.Tags
						.FirstOrDefaultAsync(t => t.Name == tagName, cancellationToken);
					if (tag == null)
					{
						tag = new Tag { Name = tagName };
						_context.Tags.Add(tag);
					}

					post.Tags.Add(tag);
				}

				_context.Posts.Add(post);
			}

			return await _context.SaveChangesAsync(cancellationToken) > 0;
		}



		// Tạo tên định danh (slug)
		private static string GenerateSlug( string phrase)
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


		public async Task<bool> IsTagSlugExistedAsync(
		int tagId, string tagSlug,
		CancellationToken cancellationToken = default)
		{
			return await _context.Set<Tag>()
				.AnyAsync(x => x.Id != tagId && x.UrlSlug == tagSlug, cancellationToken);
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
                if (post.PostedDate >= now.AddMonths(-months))
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


		// Chuyển đổi trạng thái Published của bài viết. 
		public async Task<bool> TogglePublishedFlagAsync(
        int postId, CancellationToken cancellationToken = default)
        {
            var post = await _context.Set<Post>().FindAsync(postId);

            if (post is null) return false;

            post.Published = !post.Published;
            await _context.SaveChangesAsync(cancellationToken);

            return post.Published;
        }


		


		// Tìm và phân trang các bài viết thỏa mãn điều kiện tìm kiếm 
		
		public async Task<IPagedList<Post>> GetPagedPostsAsync(
		PostQuery condition,
		int pageNumber = 1,
		int pageSize = 10,
		CancellationToken cancellationToken = default)
		{
			return await FilterPosts(condition).ToPagedListAsync(
				pageNumber, pageSize,
				nameof(Post.PostedDate), "DESC",
				cancellationToken);
		}


		public async Task<IPagedList<Tag>> GetPagedTagsAsync(
		CategoryQuery condition,
		int pageNumber = 1,
		int pageSize = 10,
		CancellationToken cancellationToken = default)
		{
			return await FilterTags(condition).ToPagedListAsync(
				pageNumber, pageSize,
				nameof(Tag.Name), "DESC",
				cancellationToken);
		}

		// t.
		public async Task<IPagedList<T>> GetPagedPostsAsync<T>(
		PostQuery condition,
		IPagingParams pagingParams,
		Func<IQueryable<Post>, IQueryable<T>> mapper)
		{
			var posts = FilterPosts(condition);
			var projectedPosts = mapper(posts);

			return await projectedPosts.ToPagedListAsync(pagingParams);
		}

		private IQueryable<Post> FilterPosts(PostQuery condition)
		{
			IQueryable<Post> posts = _context.Set<Post>()
			.Include(x => x.Category)
			.Include(x => x.Author)
			.Include(x => x.Tags);

			if (condition.PublishedOnly)
			{
				posts = posts.Where(x => x.Published);
			}

			if (condition.NotPublished)
			{
				posts = posts.Where(x => !x.Published);
			}

			if (condition.CategoryId > 0)
			{
				posts = posts.Where(x => x.CategoryId == condition.CategoryId);
			}

			if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
			{
				posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
			}

			if (condition.AuthorId > 0)
			{
				posts = posts.Where(x => x.AuthorId == condition.AuthorId);
			}

			if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
			{
				posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
			}

			if (!string.IsNullOrWhiteSpace(condition.TagSlug))
			{
				posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
			}

			if (!string.IsNullOrWhiteSpace(condition.Keyword))
			{
				posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
										 x.ShortDescription.Contains(condition.Keyword) ||
										 x.Description.Contains(condition.Keyword) ||
										 x.Category.Name.Contains(condition.Keyword) ||
										 x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
			}

			if (condition.Year > 0)
			{
				posts = posts.Where(x => x.PostedDate.Year == condition.Year);
			}

			if (condition.Month > 0)
			{
				posts = posts.Where(x => x.PostedDate.Month == condition.Month);
			}
            
            if (condition.Day > 0)
			{
				posts = posts.Where(x => x.PostedDate.Day == condition.Day);
			}

			if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
			{
				posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
			}

			return posts;
		}

		

		private IQueryable<Tag> FilterTags(CategoryQuery condition)
		{
			IQueryable<Tag> tags = _context.Tags;

			if (!string.IsNullOrWhiteSpace(condition.Keyword))
			{
				tags = tags.Where(c => c.Name.Contains(condition.Keyword) ||
										 c.Description.Contains(condition.Keyword));
			}

			if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
			{
				tags = tags.Where(c => c.UrlSlug.Contains(condition.CategorySlug));
			}

			return tags;
		}


		public async Task<List<Post>> GetRelatedPostsAsync(int postId, int categoryId)
		{
			var relatedPosts = await _context.Posts
				.Where(p => p.Id != postId && p.Category.Id == categoryId && p.Published)
				.OrderByDescending(p => p.PostedDate)
				.Take(5)
				.ToListAsync();

			return relatedPosts;
		}


		public async Task<IList<Post>> GetPostsByMonthAsync(int year, int month)
		{
			var posts = await _context.Posts
				.Where(p => p.PostedDate.Year == year && p.PostedDate.Month == month)
				.ToListAsync();
			return posts;
		}

		public async Task<bool> SetImageUrlAsync(
		int postId, string imageUrl,
		CancellationToken cancellationToken = default)
		{
			return await _context.Posts
				.Where(x => x.Id == postId)
				.ExecuteUpdateAsync(x =>
					x.SetProperty(a => a.ImageUrl, a => imageUrl),
					cancellationToken) > 0;
		}
	}
}
