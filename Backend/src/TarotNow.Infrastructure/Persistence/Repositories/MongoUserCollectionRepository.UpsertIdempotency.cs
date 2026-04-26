using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoUserCollectionRepository
{
    private static bool HasAppliedOperationKey(UserCollectionDocument existingDoc, string? normalizedOperationKey)
    {
        return normalizedOperationKey is not null
               && existingDoc.AppliedOperationKeys!.Contains(normalizedOperationKey, StringComparer.Ordinal);
    }

    private static void AppendOperationKey(UserCollectionDocument updatedDoc, string? normalizedOperationKey)
    {
        if (normalizedOperationKey is null)
        {
            return;
        }

        updatedDoc.AppliedOperationKeys!.Add(normalizedOperationKey);
        if (updatedDoc.AppliedOperationKeys.Count <= MaxAppliedOperationKeyHistory)
        {
            return;
        }

        var removeCount = updatedDoc.AppliedOperationKeys.Count - MaxAppliedOperationKeyHistory;
        updatedDoc.AppliedOperationKeys.RemoveRange(0, removeCount);
    }

    private static void ValidateUpsertInputs(Guid userId, int cardId, decimal expToGain)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId is required.", nameof(userId));
        }

        if (cardId < 0)
        {
            throw new ArgumentException("CardId must be greater than or equal to 0.", nameof(cardId));
        }

        if (expToGain < 0m)
        {
            throw new ArgumentException("Exp must be greater than or equal to 0.", nameof(expToGain));
        }
    }

    private static string? NormalizeOperationKey(string? operationKey)
    {
        return string.IsNullOrWhiteSpace(operationKey)
            ? null
            : operationKey.Trim();
    }
}
