

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface ICardsCatalogRepository
{
        Task<CardCatalogDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<CardCatalogDto?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

        Task<IEnumerable<CardCatalogDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

public class CardCatalogDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string NameVi { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string NameZh { get; set; } = string.Empty;
    public string Arcana { get; set; } = string.Empty; 
    public string? Suit { get; set; } 
    public string Element { get; set; } = string.Empty; 
    public int Number { get; set; }
    public string? ImageUrl { get; set; }

    public List<string> UprightKeywords { get; set; } = new();
    public string UprightDescription { get; set; } = string.Empty;
    public List<string> ReversedKeywords { get; set; } = new();
    public string ReversedDescription { get; set; } = string.Empty;
}
