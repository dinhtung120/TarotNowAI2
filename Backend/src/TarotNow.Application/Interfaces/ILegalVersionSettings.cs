namespace TarotNow.Application.Interfaces;

// Contract cấu hình phiên bản tài liệu pháp lý để ép người dùng chấp thuận đúng bản phát hành.
public interface ILegalVersionSettings
{
    // Phiên bản Điều khoản dịch vụ hiện hành.
    string TOSVersion { get; }

    // Phiên bản Chính sách quyền riêng tư hiện hành.
    string PrivacyVersion { get; }

    // Phiên bản tuyên bố miễn trừ trách nhiệm cho nội dung AI.
    string AiDisclaimerVersion { get; }
}
