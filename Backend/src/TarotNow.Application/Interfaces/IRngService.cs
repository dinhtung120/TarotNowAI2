namespace TarotNow.Application.Interfaces;

public interface IRngService
{
    /// <summary>
    /// Thực hiện xào bài (Fisher-Yates Shuffle).
    /// </summary>
    /// <param name="deckSize">Số lượng lá bài trong bộ bài (vd 78 lá Tarot)</param>
    /// <returns>Mảng index lá bài đã xào, từ 0 đến deckSize - 1</returns>
    int[] ShuffleDeck(int deckSize = 78);
}
