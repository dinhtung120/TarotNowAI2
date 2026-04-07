

using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Reader.Queries.GetMyReaderRequest;

public class GetMyReaderRequestQuery : IRequest<GetMyReaderRequestResult>
{
    public Guid UserId { get; set; }
}

public class GetMyReaderRequestResult
{
        public bool HasRequest { get; set; }
    
        public string? Status { get; set; }
    
    public string? IntroText { get; set; }
    
        public string? AdminNote { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
}

public class GetMyReaderRequestQueryHandler : IRequestHandler<GetMyReaderRequestQuery, GetMyReaderRequestResult>
{
    private readonly IReaderRequestRepository _readerRequestRepository;

    public GetMyReaderRequestQueryHandler(IReaderRequestRepository readerRequestRepository)
    {
        _readerRequestRepository = readerRequestRepository;
    }

    public async Task<GetMyReaderRequestResult> Handle(GetMyReaderRequestQuery request, CancellationToken cancellationToken)
    {
        
        var latestRequest = await _readerRequestRepository.GetLatestByUserIdAsync(request.UserId.ToString(), cancellationToken);
        
        
        if (latestRequest == null)
        {
            return new GetMyReaderRequestResult { HasRequest = false };
        }

        
        return new GetMyReaderRequestResult
        {
            HasRequest = true,
            Status = latestRequest.Status,
            IntroText = latestRequest.IntroText,
            AdminNote = latestRequest.AdminNote,
            CreatedAt = latestRequest.CreatedAt,
            ReviewedAt = latestRequest.ReviewedAt
        };
    }
}
