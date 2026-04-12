using MediatR;
using TarotNow.Application.Common.UserImageUpload;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Community.Commands.UploadPostImage;

// Handler upload ảnh bài viết qua pipeline chung (nén AVIF/WebP + object storage).
public class UploadPostImageCommandHandler : IRequestHandler<UploadPostImageCommand, UploadPostImageResult>
{
    private readonly IUserImagePipeline _userImagePipeline;

    public UploadPostImageCommandHandler(IUserImagePipeline userImagePipeline)
    {
        _userImagePipeline = userImagePipeline;
    }

    public async Task<UploadPostImageResult> Handle(UploadPostImageCommand request, CancellationToken cancellationToken)
    {
        var result = await _userImagePipeline.ProcessUploadAsync(
            request.ImageStream,
            request.FileName,
            request.ContentType,
            UserImageUploadKind.CommunityPost,
            cancellationToken);

        return new UploadPostImageResult(result.PublicUrl, result.PublicId);
    }
}
