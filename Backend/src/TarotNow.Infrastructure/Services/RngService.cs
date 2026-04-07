

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services;

public class RngService : IRngService
{
        public int[] ShuffleDeck(int deckSize = 78)
    {
        
        var deck = new int[deckSize];
        for (int i = 0; i < deckSize; i++)
        {
            deck[i] = i;
        }

        
        for (int i = deckSize - 1; i > 0; i--)
        {
            
            int j = RandomNumberGenerator.GetInt32(i + 1);
            
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }

        return deck;
    }

        public GachaRngResult WeightedSelect(System.Collections.Generic.IEnumerable<WeightedItem> items, string? seedForAudit = null)
    {
        var itemList = items.ToList();
        if (!itemList.Any())
            throw new System.InvalidOperationException("Cannot select from an empty item list.");

        int totalWeight = itemList.Sum(i => i.WeightBasisPoints);
        if (totalWeight <= 0)
            throw new System.InvalidOperationException("Total weight must be positive.");

        
        
        byte[] entropy = new byte[16];
        RandomNumberGenerator.Fill(entropy);
        string generatedSeed = Convert.ToHexString(entropy);

        int randomPoint = RandomNumberGenerator.GetInt32(totalWeight);

        int currentSum = 0;
        foreach (var item in itemList)
        {
            currentSum += item.WeightBasisPoints;
            if (randomPoint < currentSum)
            {
                return new GachaRngResult
                {
                    SelectedItemId = item.ItemId,
                    RngSeed = generatedSeed
                };
            }
        }

        
        return new GachaRngResult
        {
            SelectedItemId = itemList.Last().ItemId,
            RngSeed = generatedSeed
        };
    }
}
