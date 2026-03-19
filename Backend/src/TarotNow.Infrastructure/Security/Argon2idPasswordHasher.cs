/*
 * FILE: Argon2idPasswordHasher.cs
 * MỤC ĐÍCH: Implementation băm/xác minh mật khẩu bằng thuật toán Argon2id.
 *
 *   TẠI SAO ARGON2ID THAY VÌ BCRYPT?
 *   → Argon2id là CHUẨN MÃ HÓA MẬT KHẨU HIỆN ĐẠI (RFC 9106, thắng Password Hashing Competition 2015).
 *   → Kết hợp ưu điểm của Argon2i (chống side-channel) và Argon2d (chống GPU).
 *   → Memory-hard: đòi hỏi NHIỀU RAM → GPU/ASIC rất khó brute-force (bcrypt chỉ CPU-hard).
 *   → Tham số mặc định của thư viện Isopoh đã tối ưu (64MB RAM, 3 iterations, 4 parallel).
 *
 *   CÁCH HOẠT ĐỘNG:
 *   → Hash: password → salt (random) + Argon2id → chuỗi hash (chứa cả tham số + salt).
 *   → Verify: hash + password → tách tham số + salt từ hash → băm lại → so sánh.
 *   → Không cần lưu salt riêng — salt được nhúng trong chuỗi hash.
 */

using Isopoh.Cryptography.Argon2;
using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Security;

/// <summary>
/// Implement IPasswordHasher — băm mật khẩu an toàn bằng Argon2id.
/// </summary>
public class Argon2idPasswordHasher : IPasswordHasher
{
    /// <summary>
    /// Băm mật khẩu: tạo salt ngẫu nhiên + Argon2id → chuỗi hash (chứa cả config + salt).
    /// Tham số mặc định Isopoh: 64MB memory, 3 iterations, 4 parallel lanes.
    /// </summary>
    public string HashPassword(string password)
    {
        return Argon2.Hash(password);
    }

    /// <summary>
    /// Xác minh mật khẩu: tách salt + tham số từ hash → băm lại → so sánh.
    /// Trả về true nếu khớp, false nếu sai.
    /// Hàm này timing-safe (constant-time compare) → chống timing attack.
    /// </summary>
    public bool VerifyPassword(string hash, string providedPassword)
    {
        return Argon2.Verify(hash, providedPassword);
    }
}
