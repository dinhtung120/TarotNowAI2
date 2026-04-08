using MediatR;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;

// Command gửi đơn đăng ký trở thành Reader.
public class SubmitReaderRequestCommand : IRequest<bool>
{
    // Định danh user gửi đơn.
    public Guid UserId { get; set; }

    // Lời giới thiệu kinh nghiệm và định hướng hành nghề reader.
    public string IntroText { get; set; } = string.Empty;

    // Danh sách tài liệu minh chứng năng lực reader.
    public List<string> ProofDocuments { get; set; } = new();
}
