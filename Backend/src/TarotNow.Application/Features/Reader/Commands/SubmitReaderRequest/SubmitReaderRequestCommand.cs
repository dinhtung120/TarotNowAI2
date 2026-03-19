/*
 * ===================================================================
 * FILE: SubmitReaderRequestCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói Lệnh User Khởi kiện Yêu Cầu Trở Thành Người Đọc Bài (Reader).
 *   Hồ sơ bao gồm Lời Mở Đầu (Intro) và Bằng Cấp (Proof).
 * ===================================================================
 */

using MediatR;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;

/// <summary>
/// Gói Lệnh Nộp Đơn Xin Việc làm Reader.
/// 
/// LƯU Ý BẢO MẬT:
/// UserId lấy từ Token Đăng Nhập (JWT), tuyệt đối không để Frontend truyền bậy trong Body Request
/// Vì hacker có thể Fake UserId của người khác nộp đơn lừa đảo.
/// </summary>
public class SubmitReaderRequestCommand : IRequest<bool>
{
    /// <summary>Căn cước công dân của ứng viên.</summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Thư Giới Thiệu (Cover Letter) trả lời cho câu hỏi: Tại sao Tui hợp làm Reader?
    /// Có thể kèm link Facebook/Insta xem bói của họ.
    /// </summary>
    public string IntroText { get; set; } = string.Empty;

    /// <summary>
    /// File Bằng Cấp Chứng Chỉ.
    /// Frontend sẽ Upload đống ảnh này lên s3/Cloudinary trước, rồi truyền một list URL vào đây.
    /// </summary>
    public List<string> ProofDocuments { get; set; } = new();
}
