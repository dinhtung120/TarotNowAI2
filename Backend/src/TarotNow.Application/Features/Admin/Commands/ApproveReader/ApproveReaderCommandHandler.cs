

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Admin.Commands.ApproveReader;

// Handler chính điều phối luồng duyệt/từ chối đơn reader.
public partial class ApproveReaderCommandHandler : IRequestHandler<ApproveReaderCommand, bool>
{
    // Giá trị action chấp nhận cho nhánh phê duyệt.
    private const string ApproveAction = "approve";
    // Giá trị action chấp nhận cho nhánh từ chối.
    private const string RejectAction = "reject";
    // Thông điệp lỗi khi action ngoài danh sách hợp lệ.
    private const string InvalidActionMessage = "Action không hợp lệ. Chỉ chấp nhận: approve, reject.";

    private readonly IReaderRequestRepository _readerRequestRepository;
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Khởi tạo handler duyệt reader.
    /// Luồng xử lý: nhận repository request/profile/user để cập nhật đồng bộ trạng thái khi xử lý đơn.
    /// </summary>
    public ApproveReaderCommandHandler(
        IReaderRequestRepository readerRequestRepository,
        IReaderProfileRepository readerProfileRepository,
        IUserRepository userRepository)
    {
        _readerRequestRepository = readerRequestRepository;
        _readerProfileRepository = readerProfileRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Xử lý command duyệt hoặc từ chối reader request.
    /// Luồng xử lý: chuẩn hóa action, tải request và user, rẽ nhánh approve/reject tương ứng.
    /// </summary>
    public async Task<bool> Handle(ApproveReaderCommand request, CancellationToken cancellationToken)
    {
        var action = ValidateAndNormalizeAction(request.Action);
        var readerRequest = await _readerRequestRepository.GetByIdAsync(request.RequestId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy đơn xin Reader.");
        // Rule nghiệp vụ: chỉ request ở trạng thái pending mới được xử lý.
        EnsureRequestIsPending(readerRequest);

        var userId = ParseRequestUserId(readerRequest.UserId);

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy người dùng.");
        if (action == ApproveAction)
        {
            // Nhánh approve: cập nhật role/user/profile/request theo luồng phê duyệt đầy đủ.
            await HandleApproveFlowAsync(request, readerRequest, user, cancellationToken);
            return true;
        }

        // Nhánh reject: trả đơn về trạng thái bị từ chối và lưu thông tin reviewer.
        await HandleRejectFlowAsync(request, readerRequest, user, cancellationToken);
        return true;
    }

    /// <summary>
    /// Chuẩn hóa và kiểm tra giá trị action đầu vào.
    /// Luồng xử lý: trim+lowercase action, chỉ chấp nhận approve/reject, ném BadRequest nếu sai.
    /// </summary>
    private static string ValidateAndNormalizeAction(string action)
    {
        var normalizedAction = action.Trim().ToLowerInvariant();
        if (normalizedAction == ApproveAction || normalizedAction == RejectAction)
        {
            return normalizedAction;
        }

        // Rule bắt buộc action hợp lệ để tránh thao tác ngoài kịch bản nghiệp vụ đã định nghĩa.
        throw new BadRequestException(InvalidActionMessage);
    }

    /// <summary>
    /// Đảm bảo đơn reader còn ở trạng thái chờ xử lý.
    /// Luồng xử lý: kiểm tra trạng thái hiện tại, ném BadRequest nếu đơn đã xử lý trước đó.
    /// </summary>
    private static void EnsureRequestIsPending(ReaderRequestDto readerRequest)
    {
        if (readerRequest.Status == ReaderApprovalStatus.Pending)
        {
            return;
        }

        // Edge case thao tác lặp: chặn xử lý lại đơn đã duyệt/từ chối.
        throw new BadRequestException($"Đơn này đã được xử lý ({readerRequest.Status}).");
    }

    /// <summary>
    /// Parse UserId từ reader request sang Guid hợp lệ.
    /// Luồng xử lý: thử parse Guid, ném BadRequest nếu dữ liệu request bị lỗi định dạng.
    /// </summary>
    private static Guid ParseRequestUserId(string userId)
    {
        if (Guid.TryParse(userId, out var parsedUserId))
        {
            return parsedUserId;
        }

        // Edge case dữ liệu request không nhất quán: dừng sớm trước khi đụng tới user repository.
        throw new BadRequestException("Reader request chứa UserId không hợp lệ.");
    }

    /// <summary>
    /// Thực thi nhánh từ chối đơn reader.
    /// Luồng xử lý: đổi trạng thái user, cập nhật request sang rejected và lưu metadata reviewer.
    /// </summary>
    private async Task HandleRejectFlowAsync(
        ApproveReaderCommand request,
        ReaderRequestDto readerRequest,
        Domain.Entities.User user,
        CancellationToken cancellationToken)
    {
        // Đổi trạng thái user theo rule từ chối trước khi cập nhật request.
        user.RejectReaderRequest();
        await _userRepository.UpdateAsync(user, cancellationToken);

        // Đồng bộ trạng thái request và thông tin admin đã xử lý.
        readerRequest.Status = ReaderApprovalStatus.Rejected;
        readerRequest.AdminNote = request.AdminNote;
        readerRequest.ReviewedBy = request.AdminId.ToString();
        readerRequest.ReviewedAt = DateTime.UtcNow;
        await _readerRequestRepository.UpdateAsync(readerRequest, cancellationToken);
    }
}
