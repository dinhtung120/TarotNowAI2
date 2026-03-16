using MediatR;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TarotNow.Application.Features.Admin.Queries.ListDeposits;

public class ListDepositsQuery : IRequest<ListDepositsResponse>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Status { get; set; }
}

public class ListDepositsResponse
{
    [JsonPropertyName("deposits")]
    public IEnumerable<DepositDto> Deposits { get; set; } = new List<DepositDto>();

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }
}

public class DepositDto
{
    [JsonPropertyName("id")]
    public System.Guid Id { get; set; }

    [JsonPropertyName("userId")]
    public System.Guid UserId { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty; // Thêm tên người dùng để hiển thị trên Admin

    [JsonPropertyName("amountVnd")]
    public long AmountVnd { get; set; }

    [JsonPropertyName("diamondAmount")]
    public long DiamondAmount { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("transactionId")]
    public string? TransactionId { get; set; }

    [JsonPropertyName("createdAt")]
    public System.DateTime CreatedAt { get; set; }
}
