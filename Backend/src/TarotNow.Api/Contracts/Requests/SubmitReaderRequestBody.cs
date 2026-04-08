namespace TarotNow.Api.Contracts.Requests;

// Payload người dùng gửi hồ sơ đăng ký trở thành reader.
public class SubmitReaderRequestBody
{
    // Phần giới thiệu bản thân để moderator đánh giá mức độ phù hợp.
    public string IntroText { get; set; } = string.Empty;

    // Danh sách tài liệu minh chứng đính kèm cho hồ sơ.
    public List<string>? ProofDocuments { get; set; }
}
