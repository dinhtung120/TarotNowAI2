/*
 * ===================================================================
 * FILE: IRngService.cs
 * NAMESPACE: TarotNow.Application.Interfaces
 * ===================================================================
 * MỤC ĐÍCH:
 *   Giao Diện Bản Vẽ Bàn Tay Phép Thuật: Radom Number Generator (RNG).
 *   Đảm Trách Duy Nhất Khâu Xảo Nghệ Rút Lá Random Rất Chuyên Môn Để Dính Trộm Code Logic Thay Vì Sinh Auto Lụi Random.Next() Ngu Của C# Dễ Đoán.
 * ===================================================================
 */

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Tay Xào Bài RNG Xuyên Môn Điêu Luyện Không Cho Ai Dấu Bí Thuật Bên Trong (C# Layer Infrastructure Đứng Ôm Logic Thực Sự Fisher-Yates Bốc Cổ Điển Nhúng Lib Bự).
/// </summary>
public interface IRngService
{
    /// <summary>
    /// Úm Ba La Xì Bùa: Thảo Thuật Tung Bài Lên Trời (Fisher-Yates Tiên Điển Sắp Bài Chuẩn).
    /// </summary>
    /// <param name="deckSize">Xẻo Bao Nhiêu Lá Xuống Bàn Tay Từ Không Trung? 78 Là Toàn Bộ.</param>
    /// <returns>1 Dãy Chỉ Số Mới Tinh Kéo Căng Cú Ngẫu Nhiên: Vd (Lá 7, Lá 44, Lá 0...)</returns>
    int[] ShuffleDeck(int deckSize = 78);
}
