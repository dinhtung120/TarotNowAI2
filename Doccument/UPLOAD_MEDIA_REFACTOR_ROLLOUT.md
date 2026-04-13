# Upload Media Refactor Rollout (R2 Direct Upload)

## 1) Tóm tắt
Refactor upload media hiện tại chạy theo mô hình:
- FE nén ảnh/chuẩn bị media.
- FE gọi BE để lấy presigned URL.
- FE upload trực tiếp lên R2 (không đi qua Nginx/BE cho file binary).
- FE gọi BE confirm (avatar/community) hoặc gửi message payload (chat).

Áp dụng cho:
- Avatar
- Community image (post + comment)
- Chat image + chat voice

## 2) API mới / API cũ
### API mới
- `POST /api/v1/profile/avatar/presign`
- `POST /api/v1/profile/avatar/confirm`
- `POST /api/v1/community/image/presign`
- `POST /api/v1/community/image/confirm`
- `POST /api/v1/conversations/{conversationId}/media/presign`

### API cũ đã gỡ
- `POST /api/v1/profile/avatar` (multipart)
- `POST /api/v1/community/images` (multipart)

## 3) Bắt buộc cấu hình `.env`

```env
OBJECTSTORAGE_PROVIDER=R2
R2_ACCOUNT_ID=<cloudflare-account-id>
R2_ACCESS_KEY_ID=<r2-access-key-id>
R2_SECRET_ACCESS_KEY=<r2-secret-access-key>
R2_BUCKET_NAME=<r2-bucket-name>
R2_PUBLIC_BASE_URL=https://media.example.com
```

## 4) Frontend env khuyến nghị

```env
NEXT_PUBLIC_MEDIA_CDN_URL=https://img.example.com
NEXT_PUBLIC_R2_PUBLIC_URL=https://media.example.com

# Endpoint S3-compatible dùng để PUT presigned URL
NEXT_PUBLIC_R2_UPLOAD_ORIGIN=https://<account-id>.r2.cloudflarestorage.com

# Optional: thêm connect-src khác cho CSP (phân tách bằng dấu phẩy)
NEXT_PUBLIC_CSP_CONNECT_SRC_EXTRA=
```

Lưu ý:
- `NEXT_PUBLIC_R2_UPLOAD_ORIGIN` giúp CSP chỉ cho phép đúng upload endpoint của bucket/account.
- Nếu bỏ trống, frontend CSP fallback `https://*.r2.cloudflarestorage.com`.

## 5) GitHub Actions cần cập nhật gì?

## 5.1 Secrets
Không cần secret mới theo tên riêng cho R2 nếu bạn đã dùng:
- `PROD_DOTENV_PLAIN`
- hoặc `PROD_DOTENV_B64`

Chỉ cần đảm bảo nội dung `.env` trong secret đã có đầy đủ biến ở mục 3 + 4.

## 5.2 Variables (khuyến nghị thêm)
- `PROD_MEDIA_CDN_URL`
- `PROD_R2_PUBLIC_URL`
- `PROD_R2_UPLOAD_ORIGIN`

Các workflow đã đọc các biến này khi build frontend image:
- `.github/workflows/cd-main-3ec2.yml`
- `.github/workflows/cd-fast-deploy.yml`
- `.github/workflows/cd-fe-only-deploy.yml`
- `.github/workflows/ci-readiness.yml`

## 6) Root cause lỗi upload avatar vừa gặp
Lỗi thực tế không phải do BE presign (presign trả `200`), mà do CSP trên FE chặn:
- `worker-src` không cho `blob:` nên browser-image-compression worker bị block.
- `connect-src` không cho origin R2 upload endpoint nên `PUT` presigned URL bị block.

## 7) Đã fix trong code
- FE CSP thêm: `worker-src 'self' blob:`.
- FE CSP thêm connect-src cho R2 upload endpoint.
- Thêm env `NEXT_PUBLIC_R2_UPLOAD_ORIGIN` để cấu hình explicit endpoint upload.

## 8) Checklist verify sau deploy
1. Mở DevTools > Network > upload avatar.
2. Xác nhận `POST /profile/avatar/presign` trả 200.
3. Xác nhận `PUT` tới `https://<account>.r2.cloudflarestorage.com/...` trả 2xx.
4. Xác nhận `POST /profile/avatar/confirm` trả 200.
5. Không còn lỗi CSP dạng:
   - `Creating a worker from 'blob:...' violates CSP`
   - `Connecting to '...r2.cloudflarestorage.com...' violates CSP`

## 9) CORS R2 tối thiểu
- Methods: `PUT, GET, HEAD`
- Allowed Origins: domain frontend thật (`https://www.tarotnow.xyz`, staging, local)
- Allowed Headers: `content-type,x-amz-date,x-amz-content-sha256,authorization,origin`
- Expose Headers: `etag,x-amz-request-id`
