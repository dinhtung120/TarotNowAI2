using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

public partial class User
{
    public Guid Id { get; private set; }

    public string Email { get; private set; } = string.Empty;

    public string Username { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public string? AvatarUrl { get; private set; }

    public DateTime DateOfBirth { get; private set; }

    public bool HasConsented { get; private set; }

    public int Level { get; private set; } = 1;

    public long Exp { get; private set; }

    public UserWallet Wallet { get; private set; }

    public string Status { get; private set; } = string.Empty;

    public string Role { get; private set; } = string.Empty;

    public string ReaderStatus { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool MfaEnabled { get; set; }

    public string? MfaSecretEncrypted { get; set; }

    public string? MfaBackupCodesHashJson { get; set; }

    public int CurrentStreak { get; private set; } = 0;

    public DateOnly? LastStreakDate { get; private set; }

    public int PreBreakStreak { get; private set; } = 0;

        public string? ActiveTitleRef { get; private set; }

    public ICollection<UserConsent> Consents { get; private set; } = new List<UserConsent>();

    protected User()
    {
        Wallet = UserWallet.CreateDefault();
    }

    public User(
        string email,
        string username,
        string passwordHash,
        string displayName,
        DateTime dateOfBirth,
        bool hasConsented)
    {
        Id = Guid.NewGuid();
        Email = email;
        Username = username;
        PasswordHash = passwordHash;
        DisplayName = displayName;
        DateOfBirth = dateOfBirth;
        HasConsented = hasConsented;
        Status = UserStatus.Pending;
        Role = UserRole.User;
        ReaderStatus = ReaderApprovalStatus.Pending;
        Wallet = UserWallet.CreateDefault();
        CreatedAt = DateTime.UtcNow;
    }

        public void SetActiveTitle(string? titleRef)
    {
        ActiveTitleRef = titleRef;
        UpdatedAt = DateTime.UtcNow;
    }
}
