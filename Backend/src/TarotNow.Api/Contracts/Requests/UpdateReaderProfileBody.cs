namespace TarotNow.Api.Contracts.Requests;

public class UpdateReaderProfileBody
{
    public string? BioVi { get; set; }

    public string? BioEn { get; set; }

    public string? BioZh { get; set; }

    public long? DiamondPerQuestion { get; set; }

    public List<string>? Specialties { get; set; }
}
