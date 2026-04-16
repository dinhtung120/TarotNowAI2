namespace TarotNow.Application.Features.Gamification.Dtos;

// DTO chỉ số sức mạnh của một card.
public class CardStatsDto
{
    // Định danh card.
    public int CardId { get; set; }

    // Cấp độ card.
    public int Level { get; set; }

    // Chỉ số tấn công.
    public decimal Atk { get; set; }

    // Chỉ số phòng thủ.
    public decimal Def { get; set; }

    // Tổng sức mạnh quy đổi từ ATK + DEF.
    public decimal TotalPower => Atk + Def;
}
