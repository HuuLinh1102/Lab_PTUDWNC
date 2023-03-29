using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extentions;

namespace TatBlog.Services.Blogs
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly BlogDbContext _context;

        public SubscriberRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task SubscribeAsync(
            string email, 
            CancellationToken cancellationToken = default)
        {
            var existingSubscriber = await _context.Subscribers
                .FirstOrDefaultAsync(s => s.Email == email, cancellationToken);

            if (existingSubscriber != null)
            {
                return;
            }

            var subscriber = new Subscriber { Email = email };
            _context.Subscribers.Add(subscriber);

            await _context.SaveChangesAsync(cancellationToken);
        }


		public async Task UnsubscribeAsync(
            string email, 
            string reason, 
            bool voluntary, 
            CancellationToken cancellationToken = default)
		{
			var subscriber = await _context.Subscribers
                .FirstOrDefaultAsync(s => s.Email == email, cancellationToken);

			if (subscriber != null)
			{
				subscriber.UnsubscribeDate = DateTime.Now;
				subscriber.UnsubscribeReason = reason;
				subscriber.IsUserInitiatedUnsubscribe = voluntary;

				await _context.SaveChangesAsync(cancellationToken);
			}
		}

		public async Task BlockSubscriberAsync(
			int id, 
			string reason, 
			string notes, 
			CancellationToken cancellationToken = default)
		{
			var subscriber = await _context.Subscribers.FindAsync(id);

			if (subscriber != null)
			{
				subscriber.UnsubscribeDate = DateTime.Now;
				subscriber.UnsubscribeReason = reason;
				subscriber.IsUserInitiatedUnsubscribe = false;
				subscriber.AdminNote = notes;

				await _context.SaveChangesAsync(cancellationToken);
			}
		}

		public async Task DeleteSubscriberAsync(
			int id, 
			CancellationToken cancellationToken = default)
		{
			var subscriber = await _context.Subscribers.FindAsync(id);

			if (subscriber != null)
			{
				_context.Subscribers.Remove(subscriber);

				await _context.SaveChangesAsync(cancellationToken);
			}
		}

		public async Task<Subscriber> GetSubscriberByIdAsync(
			int id, 
			CancellationToken cancellationToken = default)
		{
			return await _context.Subscribers.FindAsync(id);
		}

		public async Task<Subscriber> GetSubscriberByEmailAsync(
			string email, 
			CancellationToken cancellationToken = default)
		{
			return await _context.Subscribers
				.FirstOrDefaultAsync(s => s.Email == email, cancellationToken);
		}

		public async Task<IPagedList<Subscriber>> SearchSubscribersAsync(
			IPagingParams pagingParams, 
			string keyword, 
			bool? unsubscribed, 
			bool? involuntary, 
			CancellationToken cancellationToken = default)
		{
			var query = _context.Subscribers.AsQueryable();

			if (!string.IsNullOrWhiteSpace(keyword))
			{
				query = query.Where(s => s.Email.Contains(keyword));
			}

			if (unsubscribed.HasValue)
			{
				if (unsubscribed.Value)
				{
					query = query.Where(s => s.UnsubscribeDate.HasValue);
				}
				else
				{
					query = query.Where(s => !s.UnsubscribeDate.HasValue);
				}
			}

			if (involuntary.HasValue)
			{
				query = query.Where(s => s.IsUserInitiatedUnsubscribe != involuntary.Value);
			}

			query = query.OrderByDescending(s => s.SubscriptionDate);

			// Paginate results
			var subscribers = await query
				.ToPagedListAsync(pagingParams, cancellationToken);

			return subscribers;
		}


	}
}
