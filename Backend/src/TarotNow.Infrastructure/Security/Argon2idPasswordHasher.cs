using Isopoh.Cryptography.Argon2;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Security;

/// <summary>
/// Triển khai IPasswordHasher bằng thuật toán Argon2id.
/// Chuẩn mã hóa hiện đại, an toàn hơn Bcrypt, 
/// giúp chống lại các cuộc tấn công Brute-force từ GPU.
/// </summary>
public class Argon2idPasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        // Argon2.Hash có tham số mặc định được tối ưu cho bảo mật (mức RAM, iterations, parallels)
        return Argon2.Hash(password);
    }

    public bool VerifyPassword(string hash, string providedPassword)
    {
        return Argon2.Verify(hash, providedPassword);
    }
}
