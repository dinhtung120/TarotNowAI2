# BE Community

## Source đã đọc thủ công

- Feature: `Backend/src/TarotNow.Application/Features/Community`
- Controllers: `CommunityController.cs`, `CommunityController.Posts.cs`, `CommunityController.Comments.cs`, `CommunityController.ReactionsReports.cs`, `CommunityController.MediaUpload.cs`, `AdminCommunityController.cs`
- Tests: `CreatePostCommandHandlerTests.cs`, `ToggleReactionCommandHandlerTests.cs`, `PresignCommunityImageCommandHandlerTests.cs`, `ConfirmCommunityImageCommandHandlerTests.cs`, `ResolvePostReportCommandHandlerTests.cs`
- Datastore: `MongoDbContext.cs` collections `community_posts`, `community_reactions`, `community_comments`, `community_media_assets`, `upload_sessions`, `reports`
- Domain event: `CommunityMediaAttachRequestedDomainEvent.cs`

## Entry points & luồng chính

`CommunityController.cs` là authenticated community API với policy `AuthenticatedUser` và rate limit `auth-session`.

Post flow đã đọc:

- `POST posts`: `CreatePostCommand`, rate limit `community-write`, author id lấy từ token.
- `GET posts`: `GetFeedQuery` với `ViewerId` từ token để handler áp dụng visibility.
- `GET posts/{id}`: `GetPostDetailQuery` với viewer id.
- `PUT posts/{id}`: `UpdatePostCommand`, author id từ token.
- `DELETE posts/{id}`: `DeletePostCommand`, requester id + role từ claims.

Các partial khác xử lý comments, reactions/reports và media upload presign/confirm.

## Dependency và dữ liệu

Community là MongoDB document module:

- `community_posts`: post content/visibility/author.
- `community_comments`: comments.
- `community_reactions`: reactions.
- `community_media_assets` + `upload_sessions`: media upload lifecycle.
- `reports`: report/moderation data.

Visibility/authorization phụ thuộc viewer id và requester role được truyền xuống handler; controller không tự quyết định business rule ngoài lấy context.

## Boundary / guard

- Tất cả write actions phải lấy author/requester từ token, không từ payload.
- Feed/detail phải giữ viewer context để không leak private/hidden posts.
- Media presign/confirm phải validate upload ownership/expiry/content type trong handlers.
- Report/admin resolution phải tránh bypass moderation workflow.

## Test coverage hiện có

- `CreatePostCommandHandlerTests.cs`: create post command.
- `ToggleReactionCommandHandlerTests.cs`: reaction toggle.
- `PresignCommunityImageCommandHandlerTests.cs` và `ConfirmCommunityImageCommandHandlerTests.cs`: media upload flow.
- `ResolvePostReportCommandHandlerTests.cs`: admin/moderation report resolution.

Không thấy API integration test riêng cho CommunityController trong evidence đã đọc; nếu audit sâu không tìm thêm, đây là gap P1 cho auth/visibility/media route contract.

## Rủi ro

- P0: private/hidden post leak qua feed/detail; user sửa/xóa post của người khác; upload confirm attach asset không thuộc user/post.
- P1: report/moderation path thiếu audit; media upload session không expiry/ownership check.
- P2: docs nhầm community reports với generic global reports mà không chỉ rõ collection `reports` dùng chung.

## Kết luận

Community là authenticated Mongo document module với visibility, ownership và media upload là vùng rủi ro chính. Review đúng phải đọc partial controller cụ thể, command/query handler và media/report tests tương ứng.
