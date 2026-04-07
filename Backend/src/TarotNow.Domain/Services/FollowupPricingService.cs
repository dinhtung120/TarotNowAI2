

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace TarotNow.Domain.Services;

public class FollowupPricingService
{
    
    private static readonly int[] PriceTiers = new[] { 1, 2, 4, 8, 16 };
    
    public const int MAX_FOLLOWUPS_ALLOWED = 5;

    
    public int GetMockCardLevel(int cardId)
    {
        if (cardId < 0 || cardId > 77) return 1; 

        
        if (cardId <= 21)
        {
            
            return 10 + cardId;
        }
        else
        {
            
            return (cardId % 14) + 1; 
        }
    }

        public int CalculateFreeSlotsAllowed(string cardsDrawnJson)
    {
        if (string.IsNullOrWhiteSpace(cardsDrawnJson)) return 0; 

        try
        {
            
            var cardIds = JsonSerializer.Deserialize<int[]>(cardsDrawnJson) ?? Array.Empty<int>();
            if (!cardIds.Any()) return 0;

            
            int highestLevel = cardIds.Max(GetMockCardLevel);

            if (highestLevel >= 16) return 3; 
            if (highestLevel >= 11) return 2; 
            if (highestLevel >= 6) return 1;  

            return 0; 
        }
        catch
        {
            return 0; 
        }
    }

        public int CalculateNextFollowupCost(string cardsDrawnJson, int currentFollowupCount)
    {
        
        if (currentFollowupCount >= MAX_FOLLOWUPS_ALLOWED)
        {
            throw new InvalidOperationException($"Đã đạt giới hạn tối đa {MAX_FOLLOWUPS_ALLOWED} câu hỏi phụ cho phiên rút bài này.");
        }

        
        int freeSlots = CalculateFreeSlotsAllowed(cardsDrawnJson);

        
        if (currentFollowupCount < freeSlots)
        {
            return 0; 
        }

        
        int paidTierIndex = currentFollowupCount - freeSlots;

        
        if (paidTierIndex >= PriceTiers.Length)
        {
            paidTierIndex = PriceTiers.Length - 1; 
        }

        return PriceTiers[paidTierIndex]; 
    }
}
