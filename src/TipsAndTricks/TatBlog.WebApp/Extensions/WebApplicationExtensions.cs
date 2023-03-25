using Microsoft.EntityFrameworkCore;
using NLog.Web;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Middlewares;

namespace TatBlog.WebApp.Extensions
{
	public static class WebApplicationExtensions
	{
		// Thêm các dịch vụ được yêu cầu bởi MVC Framework
		public static WebApplicationBuilder ConfigureMvc(
			this WebApplicationBuilder builder) 
		{ 
			builder.Services.AddControllersWithViews();
			builder.Services.AddResponseCompression();

			return builder;
		}

		// Cấu hình việc sử dụng NLog
		public static WebApplicationBuilder ConfigureNLog(
			this WebApplicationBuilder builder)
		{
			builder.Logging.ClearProviders();
			builder.Host.UseNLog();

			return builder;
		}

		// Đăng ký các dịch vụ với DI Container
		public static WebApplicationBuilder ConfigureServices(
			this WebApplicationBuilder builder)
		{
			builder.Services.AddDbContext<BlogDbContext>(options =>
				options.UseSqlServer(
				builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
			builder.Services.AddScoped<IBlogRepository, BlogRepository>();
			builder.Services.AddScoped<IDataSeeder, DataSeeder>();

			return builder;
		}

		// Cấu hình HTTP Repuest pipeline
		public static WebApplication UseRequestPipeline(
			this WebApplication app) 
		{
			// Thêm middleware để hiện thị thông báo lỗi
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Blog/Error");

				// Thêm middleware cho việc áp dụng HSTS( thêm header
				// Strict-Transport-Security vào HTTP Response).
				app.UseHsts();
			}
			// Thêm middleware để chuyển hướng HTTP sang HTTPS
			app.UseHttpsRedirection();
			// Thêm middleware phục vụ các yêu cầu liên quan
			// tới các tập tin nội dung tĩnh như hình ảnh, css, ..
			app.UseStaticFiles();

			// Thêm middleware lựa chọn endpoint phù hợp nhất
			// để xử lý một HTTP request
			app.UseRouting();

			// Thêm middleware để lưu vết người dùng
			app.UseMiddleware<UserActivityMiddleware>();

			return app;
		}

		// Thêm dữ liệu vào CSDL
		public static IApplicationBuilder UseDataSeeder(
			this IApplicationBuilder app) 
		{
			using var scope = app.ApplicationServices.CreateScope();

			try 
			{
				scope.ServiceProvider
					.GetService<IDataSeeder>()
					.Initialize();
			}
			catch (Exception ex) 
			{
				scope.ServiceProvider
					.GetRequiredService<ILogger<Program>>()
					.LogError(ex, "Could not insert data into database");
			}

			return app;
		}
	}

}
