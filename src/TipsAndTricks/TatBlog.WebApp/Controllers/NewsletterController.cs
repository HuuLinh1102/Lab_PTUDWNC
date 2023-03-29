using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using FluentEmail;
using FluentEmail.Smtp;

namespace TatBlog.WebApp.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly ISubscriberRepository _subscriberRepository;

        public NewsletterController(ISubscriberRepository subscriberRepository)
        {
            _subscriberRepository = subscriberRepository;
        }

        public IActionResult Index()
        {
            return PartialView("_NewsletterForm");
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(string email)
        {
            var subscriber = await _subscriberRepository.GetSubscriberByEmailAsync(email);

            if (subscriber == null)
            {
                await _subscriberRepository.SubscribeAsync(email);
				// Gửi email cảm ơn và liên kết hủy đăng ký
				var subject = "Cảm ơn bạn đã đăng ký nhận thông báo từ chúng tôi";
				var body = $"Chào mừng bạn đến với chúng tôi! " +
					$"Bạn đã đăng ký nhận thông báo từ chúng tôi với địa chỉ email là {email}. " +
					$"Nếu bạn muốn hủy đăng ký, vui lòng click vào link sau: " +
					$"{Url.Action("Unsubscribe", "Newsletter", new { email = email }, Request.Scheme)}";

				var emailService = new EmailService("smtp.gmail.com", 587, "", "");

				await emailService.SendAsync(email, subject, body);
			}

            return RedirectToAction("Index", "Blog");

        }

        public async Task<IActionResult> Unsubscribe(string email)
        {
            var subscriber = await _subscriberRepository.GetSubscriberByEmailAsync(email);

            if (subscriber != null)
            {
                subscriber.UnsubscribeDate = DateTime.UtcNow;
                subscriber.IsUserInitiatedUnsubscribe = true;

            }

            return RedirectToAction("Index", "Blog");
        }
    }

}
