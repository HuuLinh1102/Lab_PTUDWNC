using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class AuthorsController : Controller
	{
		private readonly ILogger<AuthorsController> _logger;
		private readonly IBlogRepository _blogRepository;
		private readonly IMediaManager _mediaManager;
		private readonly IMapper _mapper;

		public AuthorsController(
			ILogger<AuthorsController> logger,
			IBlogRepository blogRepository,
			IMediaManager mediaManager,
			IMapper mapper)
		{
			_logger = logger;
			_blogRepository = blogRepository;
			_mediaManager = mediaManager;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index(
			AuthorFilterModel model,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 10)
		{

			_logger.LogInformation("Tạo điều kiện truy vấn");
			var authorQuery = _mapper.Map<AuthorQuery>(model);

			_logger.LogInformation("Lấy danh sách chủ dề từ CSDL");

			ViewBag.AuthorsList = await _blogRepository
				.GetPagedAuthorsAsync(authorQuery, pageNumber, pageSize);

			_logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");


			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id = 0)
		{
			// ID = 0 <=> Thêm bài viết
			// ID > 0 : Đọc dữ liệu của bài viết từ CSDL
			var author = id > 0
				? await _blogRepository.GetAuthorByIdAsync(id)
				: null;

			// Tạo view model từ dữ liệu của bài viết
			var model = author == null
				? new AuthorEditModel()
				: _mapper.Map<AuthorEditModel>(author);


			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(
			[FromServices] IValidator<AuthorEditModel> authorValidator,
			AuthorEditModel model)
		{
			var validationResult = await authorValidator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState);
			}

			var author = model.Id > 0
				? await _blogRepository.GetAuthorByIdAsync(model.Id)
				: null;

			if (author == null)
			{
				author = _mapper.Map<Author>(model);

				author.Id = 0;
				author.JoinedDate = DateTime.Now;
			}
			else
			{
				_mapper.Map(model, author);

			}

			// Nếu người dùng có upload hình ảnh minh họa cho bài viết
			if (model.ImageFile?.Length > 0)
			{
				var newImagePath = await _mediaManager.SaveFileAsync(
					model.ImageFile.OpenReadStream(),
					model.ImageFile.FileName,
					model.ImageFile.ContentType);

				if (!string.IsNullOrWhiteSpace(newImagePath))
				{
					await _mediaManager.DeleteFileAsync(author.ImageUrl);
					author.ImageUrl = newImagePath;
				}
			}

			await _blogRepository.CreateOrUpdateAuthorAsync(author);

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> VerifyAuthorSlug(
			int id, string urlSlug)
		{
			var slugExisted = await _blogRepository
				.IsAuthorSlugExistedAsync(id, urlSlug);

			return slugExisted
				? Json($"slug '{urlSlug}' đã được sử dụng")
				: Json(true);
		}


		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var author = await _blogRepository.GetAuthorByIdAsync(id);
			if (author == null)
			{
				return NotFound();
			}

			await _blogRepository.DeleteByIdAsync<Author>(id);

			return RedirectToAction(nameof(Index));
		}

	}
}
