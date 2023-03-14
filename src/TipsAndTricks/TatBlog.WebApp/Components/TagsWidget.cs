using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
	public class TagsWidget : ViewComponent
	{
		private readonly IBlogRepository _blogRepository;

		public TagsWidget(IBlogRepository blogRepository)
		{
			_blogRepository = blogRepository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			// Lấy danh sách tag
			var tags = await _blogRepository.GetTagsAsync();

			return View(tags);
		}
	}
}
