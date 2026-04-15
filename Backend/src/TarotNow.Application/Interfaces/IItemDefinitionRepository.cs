using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Contract truy cập master data item definitions.
/// </summary>
public interface IItemDefinitionRepository
{
    /// <summary>
    /// Lấy item definition theo mã code.
    /// </summary>
    Task<ItemDefinition?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy item definition theo id.
    /// </summary>
    Task<ItemDefinition?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy toàn bộ item definition đang active.
    /// </summary>
    Task<IReadOnlyList<ItemDefinition>> GetActiveAsync(CancellationToken cancellationToken = default);
}
