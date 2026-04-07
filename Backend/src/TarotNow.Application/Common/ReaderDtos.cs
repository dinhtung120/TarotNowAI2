

namespace TarotNow.Application.Common;

public class ReaderRequestDto
{
        public string Id { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string IntroText { get; set; } = string.Empty;

        public List<string> ProofDocuments { get; set; } = new();

        public string? AdminNote { get; set; }

        public string? ReviewedBy { get; set; }

        public DateTime? ReviewedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
}

public class ReaderProfileDto
{
        public string Id { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public long DiamondPerQuestion { get; set; }

        public string BioVi { get; set; } = string.Empty;

        public string BioEn { get; set; } = string.Empty;

        public string BioZh { get; set; } = string.Empty;

        public List<string> Specialties { get; set; } = new();

        public double AvgRating { get; set; }

        public int TotalReviews { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
}
