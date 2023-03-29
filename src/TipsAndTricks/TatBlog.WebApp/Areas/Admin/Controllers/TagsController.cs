using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class TagsController : Controller
	{
		private readonly ILogger<TagsController> _logger;
		private readonly IBlogRepository _blogRepository;
		private readonly IMediaManager _mediaManager;
		private readonly IMapper _mapper;

		public TagsController(
			ILogger<TagsController> logger,
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
			TagFilterModel model,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 10)
		{

			_logger.LogInformation("Tạo điều kiện truy vấn");
			var tagQuery = _mapper.Map<CategoryQuery>(model);

			_logger.LogInformation("Lấy danh sách chủ dề từ CSDL");

			ViewBag.TagsList = await _blogRepository
				.GetPagedTagsAsync(tagQuery, pageNumber, pageSize);

			_logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");


			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id = 0)
		{
			// ID = 0 <=> Thêm tag
			// ID > 0 : Đọc dữ liệu của tag từ CSDL
			var tag = id > 0
				? await _blogRepository.FindByIdAsync<Tag>(id)
				: null;

			// Tạo view model từ dữ liệu của tag
			var model = tag == null
				? new TagEditModel()
				: _mapper.Map<TagEditModel>(tag);


			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(
			[FromServices] IValidator<TagEditModel> tagValidator,
			TagEditModel model)
		{
			var validationResult = await tagValidator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState);
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var tag = model.Id > 0
				? await _blogRepository.FindByIdAsync<Tag>(model.Id)
				: null;

			if (tag == null)
			{
				tag = _mapper.Map<Tag>(model);

				tag.Id = 0;
			}
			else
			{
				_mapper.Map(model, tag);
			}


			await _blogRepository.CreateOrUpdateTagAsync(tag);

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> VerifyTagSlug(
			int id, string urlSlug)
		{
			var slugExisted = await _blogRepository
				.IsTagSlugExistedAsync(id, urlSlug);

			return slugExisted
				? Json($"slug '{urlSlug}' đã được sử dụng")
				: Json(true);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var tag = await _blogRepository.FindByIdAsync<Tag>(id);
			if (tag == null)
			{
				return NotFound();
			}

			await _blogRepository.DeleteByIdAsync<Tag>(id);

			return RedirectToAction(nameof(Index));
		}
	}
}


