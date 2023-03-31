using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints
{
	public static class CategoryEndpoints
	{
		public static WebApplication MapCategoryEndpoints(
			this WebApplication app)
		{
			var routeGroupBuilder = app.MapGroup("/api/categories");

			routeGroupBuilder.MapGet("/", GetCategories)
				.WithName("GetCategories")
				.Produces<ApiResponse<PaginationResult<CategoryItem>>>();

			routeGroupBuilder.MapGet("/{id:int}", GetCategoryDetails)
				.WithName("GetCategoryById")
				.Produces<ApiResponse<CategoryItem>>()
				.Produces(404);

			routeGroupBuilder.MapGet(
					"/{slug:regex(^[a-z0-9 -]+$)}/posts",
					GetPostsByCategorySlug)
				.WithName("GetPostsByCategorySlug")
				.Produces<ApiResponse<PaginationResult<PostDto>>>();

			routeGroupBuilder.MapPost("/", AddCategory)
				.WithName("AddNewCategory")
				.AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
				.RequireAuthorization()
				.Produces(401)
				.Produces<ApiResponse<CategoryItem>>();

			routeGroupBuilder.MapPut("/{id:int}", UpdateCategory)
				.WithName("UpdateAnCategory")
				.AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
				.RequireAuthorization()
				.Produces(401)
				.Produces<ApiResponse<CategoryItem>>();

			routeGroupBuilder.MapDelete("/{id:int}", DeleteCategory)
				.WithName("DeleteAnCategory")
				.Produces(204)
				.Produces(404);

			return app;
		}

		private static async Task<IResult> GetCategories(
			[AsParameters] CategoryFilterModel model,
			[FromServices] ICategoryRepository categoryRepository)
		{
			var categoriesList = await categoryRepository
				.GetPagedCategoriesAsync(model, model.Name);

			var paginationResult =
				new PaginationResult<CategoryItem>(categoriesList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> GetCategoryDetails(
			int id,
			[FromServices] ICategoryRepository categoryRepository,
			[FromServices] IMapper mapper)
		{
			var category = await categoryRepository.GetCachedCategoryByIdAsync(id);
			return category == null
				? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				$"Couldn not find category with Id {id}"))
				: Results.Ok(ApiResponse.Success(mapper.Map<CategoryItem>(category)));
		}

		private static async Task<IResult> GetPostsByCategorySlug(
			string slug,
			[AsParameters] PagingModel pagingModel,
			[FromServices] IBlogRepository blogRepository)
		{
			var postQuery = new PostQuery()
			{
				CategorySlug = slug,
				PublishedOnly = true
			};

			var postsList = await blogRepository.GetPagedPostsAsync(
				postQuery, pagingModel,
				posts => posts.ProjectToType<PostDto>());

			var paginationResult = new PaginationResult<PostDto>(postsList);

			return Results.Ok(ApiResponse.Success(paginationResult));
		}

		private static async Task<IResult> AddCategory(
			[FromServices] CategoryEditModel model,
			[FromServices] ICategoryRepository categoryRepository,
			[FromServices] IMapper mapper)
		{
			if (await categoryRepository
				.IsCategorySlugExistedAsync(0, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
				$"Slug '{model.UrlSlug}' already exist"));

			}

			var category = mapper.Map<Category>(model);
			await categoryRepository.AddOrUpdateAsync(category);

			return Results.Ok(ApiResponse.Success(
				mapper.Map<CategoryItem>(category),
				HttpStatusCode.Created));
		}

		private static async Task<IResult> UpdateCategory(
			[FromRoute] int id,
			[FromServices] CategoryEditModel model,
			[FromServices] IValidator<CategoryEditModel> validator,
			[FromServices] ICategoryRepository categoryRepository,
			[FromServices] IMapper mapper)
		{
			var validationResult = await validator.ValidateAsync(model);

			if (!validationResult.IsValid)
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.BadRequest, validationResult));
			}

			if (await categoryRepository.IsCategorySlugExistedAsync(
				id, model.UrlSlug))
			{
				return Results.Ok(ApiResponse.Fail(
					HttpStatusCode.Conflict,
					$"Slug '{model.UrlSlug}' already exist"));
			}

			var category = mapper.Map<Category>(model);
			category.Id = id;

			return await categoryRepository.AddOrUpdateAsync(category)
				? Results.Ok(ApiResponse.Success("Category is updated",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find category"));
		}

		private static async Task<IResult> DeleteCategory(
			[FromRoute] int id,
			[FromServices] ICategoryRepository categoryRepository)
		{
			return await categoryRepository.DeleteCategoryAsync(id)
				? Results.Ok(ApiResponse.Success("Category is deleted",
				HttpStatusCode.NoContent))
				: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound,
				"Could not find category"));
		}
	}
}
