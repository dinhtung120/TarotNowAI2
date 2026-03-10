namespace TarotNow.Application.Interfaces;

/// <summary>
/// Interface trừu tượng hóa việc khởi tạo Argon2id.
/// Giúp code ở tầng Application không phụ thuộc trực tiếp vào package mã hóa.
/// </summary>
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string hash, string providedPassword);
}
