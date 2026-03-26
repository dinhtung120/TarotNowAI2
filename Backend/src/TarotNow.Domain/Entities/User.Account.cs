using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

public partial class User
{
    public void AddExp(long amount)
    {
        if (amount <= 0)
        {
            return;
        }

        Exp += amount;
        var newLevel = 1 + (int)(Exp / 100);
        if (newLevel > Level)
        {
            Level = newLevel;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePassword(string newHash)
    {
        PasswordHash = newHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(string displayName, string? avatarUrl, DateTime dateOfBirth)
    {
        DisplayName = displayName;
        AvatarUrl = avatarUrl;
        DateOfBirth = dateOfBirth;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Lock()
    {
        Status = UserStatus.Locked;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Unlock()
    {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void PromoteToAdmin()
    {
        Role = UserRole.Admin;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ApproveAsReader()
    {
        Role = UserRole.TarotReader;
        ReaderStatus = ReaderApprovalStatus.Approved;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RejectReaderRequest()
    {
        ReaderStatus = ReaderApprovalStatus.Rejected;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RestoreRoleAndReaderStatus(string role, string readerStatus)
    {
        Role = role;
        ReaderStatus = readerStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateRole(string newRole)
    {
        Role = newRole;
        UpdatedAt = DateTime.UtcNow;
    }
}
