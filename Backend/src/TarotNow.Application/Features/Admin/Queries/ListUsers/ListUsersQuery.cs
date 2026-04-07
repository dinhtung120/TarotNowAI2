

using MediatR;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Admin.Queries.ListUsers;

public class ListUsersQuery : IRequest<ListUsersResponse>
{
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;

        public string? SearchTerm { get; set; }
}

public class ListUsersResponse
{
        public IEnumerable<UserDto> Users { get; set; } = new List<UserDto>();

        public int TotalCount { get; set; }
}

public class UserDto
{
        public System.Guid Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    
        [System.Text.Json.Serialization.JsonPropertyName("level")]
    public int Level { get; set; }
    
        [System.Text.Json.Serialization.JsonPropertyName("exp")]
    public long Exp { get; set; }
    
        [System.Text.Json.Serialization.JsonPropertyName("goldBalance")]
    public long GoldBalance { get; set; }
    
        [System.Text.Json.Serialization.JsonPropertyName("diamondBalance")]
    public long DiamondBalance { get; set; }
    
        public System.DateTime CreatedAt { get; set; }
}
