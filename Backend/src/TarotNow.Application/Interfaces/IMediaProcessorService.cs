

using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IMediaProcessorService
{
        Task<(byte[] Data, string MimeType)> ProcessAndCompressImageAsync(byte[] imageBytes, CancellationToken cancellationToken = default);

        Task<(byte[] Data, string MimeType)> ProcessAndCompressVoiceAsync(byte[] voiceBytes, string extension, CancellationToken cancellationToken = default);
}
