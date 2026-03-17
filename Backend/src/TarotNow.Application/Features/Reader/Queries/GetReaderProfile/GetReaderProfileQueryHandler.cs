using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Queries.GetReaderProfile;

/// <summary>
/// Handler trả về ReaderProfileDto từ repository.
/// Trả về null nếu user chưa có profile — caller (Controller) trả 404.
/// </summary>
public class GetReaderProfileQueryHandler : IRequestHandler<GetReaderProfileQuery, ReaderProfileDto?>
{
    private readonly IReaderProfileRepository _readerProfileRepository;

    public GetReaderProfileQueryHandler(IReaderProfileRepository readerProfileRepository)
    {
        _readerProfileRepository = readerProfileRepository;
    }

    public async Task<ReaderProfileDto?> Handle(GetReaderProfileQuery request, CancellationToken cancellationToken)
    {
        return await _readerProfileRepository.GetByUserIdAsync(request.UserId, cancellationToken);
    }
}
