namespace TarotNow.Application.Features.Gamification.Dtos;

public class CardStatsDto
{
    public int CardId { get; set; }
    public int Level { get; set; }
    public int Atk { get; set; }
    public int Def { get; set; }
    public int TotalPower => Atk + Def;
}
