# R2 Direct Upload Guide (FE-first + Presigned URL)

## 1. Scope
Flow mới áp dụng cho:
- Avatar: `/api/v1/profile/avatar/presign` + `/api/v1/profile/avatar/confirm`
- Community image (post/comment): `/api/v1/community/image/presign` + `/api/v1/community/image/confirm`
- Chat media (image/voice): `/api/v1/conversations/{conversationId}/media/presign` + gửi message payload `url + objectKey + uploadToken`

Upload binary không đi qua Backend nữa. Browser upload trực tiếp lên Cloudflare R2 qua `PUT` presigned URL.

## 2. Runtime Flow
### Avatar
1. FE validate `image/*` max 10MB.
2. FE nén bằng `browser-image-compression` thành `image/webp`.
3. FE gọi presign avatar.
4. FE `PUT` trực tiếp lên R2 (XMLHttpRequest + progress + retry).
5. FE gọi confirm avatar để cập nhật DB và xóa ảnh cũ.

### Community image (post/comment)
1. FE tạo `contextDraftId` cho draft post/comment.
2. FE nén ảnh, gọi presign community image theo `contextType=post|comment`.
3. FE upload lên R2.
4. FE gọi confirm để tạo asset trạng thái `uploaded`.
5. Khi create/update post/comment, BE attach asset theo object key trong markdown.

### Chat image/voice
1. FE nén ảnh hoặc lấy voice blob recorder.
2. FE gọi presign chat media (`mediaKind=image|voice`).
3. FE upload trực tiếp lên R2.
4. FE gửi message với `mediaPayload.url`, `mediaPayload.objectKey`, `mediaPayload.uploadToken`.
5. BE validate + consume upload token one-time trong `SendMessage`.

## 3. Cloudflare R2 CORS (Required)
Thiết lập CORS cho bucket R2 để browser upload được từ web origin.

### Methods
- `PUT`
- `GET`
- `HEAD`

### Allowed Origins
- Chỉ whitelist domain frontend thực tế (prod/staging/local).
- Không dùng wildcard `*` cho production.

### Allowed Headers
- `content-type`
- `x-amz-date`
- `x-amz-content-sha256`
- `authorization`
- `origin`

### Expose Headers (optional)
- `etag`
- `x-amz-request-id`

### Max Age
- 3600s hoặc theo policy hạ tầng.

## 4. Security Rules
- Presigned URL TTL: 10 phút.
- Upload token: one-time, consume ngay khi confirm/send-message.
- Scope cứng theo nghiệp vụ: `avatar`, `community_image`, `chat_image`, `chat_voice`.
- Image bắt buộc `image/webp` cho avatar/community/chat-image.
- Voice giới hạn size/duration theo backend constants.
- Object key prefix bắt buộc theo từng module:
  - `avatars/...`
  - `community/post/...` hoặc `community/comment/...`
  - `chat/{conversationId}/images/...`
  - `chat/{conversationId}/voices/...`
- FE chỉ render URL media thuộc domain whitelist app cấu hình.
- Endpoint legacy multipart đã hard-cutover, không nhận file binary mới qua API.

## 5. Cleanup & Asset Lifecycle
- Upload session hết hạn chưa consume sẽ được cleanup job xóa object R2.
- Community asset không attach sau TTL sẽ chuyển orphaned và bị cleanup.
- Avatar confirm giữ logic xóa ảnh cũ trên R2 (best effort + warning log nếu lỗi).

## 6. Mobile Compatibility
- Upload dùng `XMLHttpRequest` để có progress thật trên iOS Safari + Android Chrome.
- Nén ảnh dùng `browser-image-compression` với `useWebWorker: true` để giảm block UI.
- FE có optimistic UI + rollback khi upload/confirm thất bại.
