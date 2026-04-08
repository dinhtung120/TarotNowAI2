using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.UnitTests.Reading;

// Unit test cho tập hằng trạng thái kết thúc stream AI.
public class AiStreamFinalStatusesTests
{
    /// <summary>
    /// Xác nhận các giá trị hằng của AiStreamFinalStatuses khớp domain constants.
    /// Luồng này bảo vệ contract trạng thái dùng xuyên suốt application và domain.
    /// </summary>
    [Fact]
    public void Values_ShouldMatchDomainAiRequestStatusConstants()
    {
        Assert.Equal(AiRequestStatus.Completed, AiStreamFinalStatuses.Completed);
        Assert.Equal(AiRequestStatus.FailedBeforeFirstToken, AiStreamFinalStatuses.FailedBeforeFirstToken);
        Assert.Equal(AiRequestStatus.FailedAfterFirstToken, AiStreamFinalStatuses.FailedAfterFirstToken);
    }

    /// <summary>
    /// Xác nhận hàm IsSupported chỉ chấp nhận các trạng thái kết thúc hợp lệ.
    /// Luồng này kiểm tra cả giá trị hợp lệ, không hợp lệ và null/rỗng.
    /// </summary>
    [Theory]
    [InlineData("completed", true)]
    [InlineData("failed_before_first_token", true)]
    [InlineData("failed_after_first_token", true)]
    [InlineData("failed", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsSupported_ShouldValidateKnownValues(string? status, bool expected)
    {
        Assert.Equal(expected, AiStreamFinalStatuses.IsSupported(status));
    }
}
