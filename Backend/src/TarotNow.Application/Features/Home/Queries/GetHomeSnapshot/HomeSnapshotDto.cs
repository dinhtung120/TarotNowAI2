using System.Collections.Generic;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Home.Queries.GetHomeSnapshot;

public class HomeSnapshotDto
{
    public IReadOnlyList<ReaderProfileDto> FeaturedReaders { get; set; } = new List<ReaderProfileDto>();

    public long TotalCount { get; set; }
}
