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
    public class CategoriesController : Controller
    {
		private readonly ILogger<CategoriesController> _logger;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMediaManager _mediaManager;
		private readonly IMapper _mapper;

		public CategoriesController(
			ILogger<CategoriesController> logger,
			ICategoryRepository blogRepository,
			IMediaManager mediaManager,
			IMapper mapper)
		{
			_logger = logger;
			_categoryRepository = blogRepository;
			_mediaManager = mediaManager;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index(
			CategoryFilterModel model,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 10)
		{

			_logger.LogInformation("Tạo điều kiện truy vấn");
			var categoryQuery = _mapper.Map<CategoryQuery>(model);

			_logger.LogInformation("Lấy danh sách chủ dề từ CSDL");

			ViewBag.CategoriesList = await _categoryRepository
				.GetPagedCategoriesQueryAsync(categoryQuery, pageNumber, pageSize);
			
			_logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");

		
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id = 0)
		{
			// ID = 0 <=> Thêm bài viết
			// ID > 0 : Đọc dữ liệu của bài viết từ CSDL
			var category = id > 0
				? await _categoryRepository.GetCategoryByIdAsync(id)
				: null;

			// Tạo view model từ dữ liệu của bài viết
			var model = category == null
				? new CategoryEditModel()
				: _mapper.Map<CategoryEditModel>(category);


			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(
			[FromServices] IValidator<CategoryEditModel> categoryValidator,
			CategoryEditModel model)
		{
			var validationResult = await categoryValidator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				validationResult.AddToModelState(ModelState);
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var category = model.Id > 0
				? await _categoryRepository.GetCategoryByIdAsync(model.Id)
				: null;

			if (category == null)
			{
				category = _mapper.Map<Category>(model);

				category.Id = 0;
			}
			else
			{
				_mapper.Map(model, category);
			}


			await _categoryRepository.AddOrUpdateAsync(category);

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> VerifyCategorySlug(
			int id, string urlSlug)
		{
			var slugExisted = await _categoryRepository
				.IsCategorySlugExistedAsync(id, urlSlug);

			return slugExisted
				? Json($"slug '{urlSlug}' đã được sử dụng")
				: Json(true);
		}


		[HttpPost]
		public async Task<IActionResult> ShowOnMenu(int id)
		{
			var category = await _categoryRepository.GetCategoryByIdAsync(id);
			if (category == null)
			{
				return NotFound();
			}
			await _categoryRepository.ToggleShowOnMenuFlagAsync(id);

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var category = await _categoryRepository.GetCategoryByIdAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			await _categoryRepository.DeleteCategoryAsync(id);

			return RedirectToAction(nameof(Index));
		}
	}
}


