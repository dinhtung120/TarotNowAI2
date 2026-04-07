

using TarotNow.Application.Features.Admin.Queries;
using TarotNow.Application.Features.Admin.Queries.GetLedgerMismatch;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using Moq;
using FluentAssertions;

namespace TarotNow.Application.UnitTests.Admin;

public class GetLedgerMismatchQueryHandlerTests
{
    private readonly Mock<IAdminRepository> _mockAdminRepository;
    private readonly GetLedgerMismatchQueryHandler _handler;

    public GetLedgerMismatchQueryHandlerTests()
    {
        _mockAdminRepository = new Mock<IAdminRepository>();
        _handler = new GetLedgerMismatchQueryHandler(_mockAdminRepository.Object);
    }

        [Fact]
    public async Task Handle_WhenMismatchesExist_ReturnsMismatchList()
    {
        var query = new GetLedgerMismatchQuery();
        var mismatches = new List<MismatchRecord>
        {
            new MismatchRecord { UserId = Guid.NewGuid(), UserGoldBalance = 100, LedgerGoldBalance = 90 }
        };

        _mockAdminRepository.Setup(r => r.GetLedgerMismatchesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(mismatches);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().UserGoldBalance.Should().Be(100);
        result.First().LedgerGoldBalance.Should().Be(90);
    }

        [Fact]
    public async Task Handle_WhenNoMismatches_ReturnsEmptyList()
    {
        var query = new GetLedgerMismatchQuery();
        var emptyList = new List<MismatchRecord>();

        _mockAdminRepository.Setup(r => r.GetLedgerMismatchesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyList);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
