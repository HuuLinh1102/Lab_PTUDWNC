using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components
{
	public class BestAuthors : ViewComponent
	{
		private readonly IAuthorRepository _authorRepository;

		public BestAuthors(IAuthorRepository authorRepository)
		{
			_authorRepository = authorRepository;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			
			// Lấy danh sách tác giả
			var authors = await _authorRepository.GetPopularAuthorsAsync(4,null);

			return View(authors);
		}
	}
}
