

using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IEmailSender
{
        Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
}
