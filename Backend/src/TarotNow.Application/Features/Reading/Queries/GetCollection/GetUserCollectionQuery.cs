

using MediatR;
using TarotNow.Domain.Entities;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Reading.Queries.GetCollection;

public class GetUserCollectionQuery : IRequest<List<UserCollectionDto>>
{
    public Guid UserId { get; set; }
}

public class UserCollectionDto
{
        public int CardId { get; set; }
    
        public int Level { get; set; }
    
        public long ExpGained { get; set; }
    
        public DateTime LastDrawnAt { get; set; }
    
        public int Copies { get; set; }

        public int Atk { get; set; }

        public int Def { get; set; }
}
