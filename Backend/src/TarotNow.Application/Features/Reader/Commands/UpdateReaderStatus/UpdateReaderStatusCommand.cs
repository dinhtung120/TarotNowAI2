using MediatR;
using System;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;

// Command cập nhật trạng thái online thủ công của Reader.
public class UpdateReaderStatusCommand : IRequest<bool>
{
    // Định danh user của reader cần đổi trạng thái.
    public Guid UserId { get; set; }

    // Trạng thái mục tiêu do client gửi (offline/busy).
    public string Status { get; set; } = string.Empty;
}
