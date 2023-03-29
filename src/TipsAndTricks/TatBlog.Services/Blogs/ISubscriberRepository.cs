using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface ISubscriberRepository
    {
		Task SubscribeAsync(
			string email,
			CancellationToken cancellationToken = default);

		Task UnsubscribeAsync(
			string email,
			string reason,
			bool voluntary,
			CancellationToken cancellationToken = default);

		Task BlockSubscriberAsync(
			int id,
			string reason,
			string notes,
			CancellationToken cancellationToken = default);

		Task DeleteSubscriberAsync(
			int id,
			CancellationToken cancellationToken = default);

		Task<Subscriber> GetSubscriberByIdAsync(
			int id,
			CancellationToken cancellationToken = default);

		Task<Subscriber> GetSubscriberByEmailAsync(
			string email,
			CancellationToken cancellationToken = default);

		Task<IPagedList<Subscriber>> SearchSubscribersAsync(
			IPagingParams pagingParams,
			string keyword,
			bool? unsubscribed,
			bool? involuntary,
			CancellationToken cancellationToken = default);
    }
}
