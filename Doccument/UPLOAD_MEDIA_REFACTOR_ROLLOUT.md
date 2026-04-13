# Upload Media Refactor Rollout (R2 Direct Upload)

## 1) Mục tiêu release
- Hard cutover toàn bộ upload media sang `FE-first + Presigned R2`.
- Áp dụng cho:
  - Avatar
  - Community image (post + comment)
  - Chat image + chat voice
- Backend không nhận binary upload nữa, chỉ `presign + confirm/consume token`.

## 2) API mới và API đã gỡ
### API mới
- `POST /api/v1/profile/avatar/presign`
- `POST /api/v1/profile/avatar/confirm`
- `POST /api/v1/community/image/presign`
- `POST /api/v1/community/image/confirm`
- `POST /api/v1/conversations/{conversationId}/media/presign`

### API cũ đã gỡ (hard cutover)
- `POST /api/v1/profile/avatar` (multipart)
- `POST /api/v1/community/images` (multipart)

## 3) Bắt buộc cấu hình .env
Dùng `.env` ở root repo (cho Docker Compose + Backend + Frontend).

### 3.1 Các biến bắt buộc cho R2
```env
OBJECTSTORAGE_PROVIDER=R2
R2_ACCOUNT_ID=<cloudflare-account-id>
R2_ACCESS_KEY_ID=<r2-access-key-id>
R2_SECRET_ACCESS_KEY=<r2-secret-access-key>
R2_BUCKET_NAME=<r2-bucket-name>
R2_PUBLIC_BASE_URL=https://media.example.com
```

### 3.2 Các biến frontend khuyến nghị (build-time whitelist domain ảnh)
```env
NEXT_PUBLIC_MEDIA_CDN_URL=https://img.example.com
NEXT_PUBLIC_R2_PUBLIC_URL=https://media.example.com
```

### 3.3 Các biến liên quan CDN ảnh catalog (nếu dùng)
```env
MEDIACDN_PUBLIC_BASE_URL=https://img.example.com
```

## 4) GitHub Actions cần cập nhật gì?

## 4.1 Secrets
Không cần tạo secret mới theo tên riêng cho R2 nếu bạn đã dùng 1 trong 2 secret env tổng:
- `PROD_DOTENV_PLAIN`
- `PROD_DOTENV_B64`

Bạn chỉ cần đảm bảo file `.env` được nhúng trong secret đó có đủ biến ở mục 3.

Lưu ý bảo mật:
- Tuyệt đối không đặt `R2_SECRET_ACCESS_KEY` trong GitHub Variables.
- Chỉ đặt trong GitHub Secrets (hoặc file env trên host, không commit).

## 4.2 Variables (khuyến nghị thêm)
Để build frontend image có whitelist domain media chính xác, thêm:
- `PROD_MEDIA_CDN_URL` (optional)
- `PROD_R2_PUBLIC_URL` (optional)

Các workflow CD đã đọc 2 biến này khi build FE image:
- `.github/workflows/cd-main-3ec2.yml`
- `.github/workflows/cd-fast-deploy.yml`
- `.github/workflows/cd-fe-only-deploy.yml`

Nếu chưa set, hệ thống fallback chuỗi rỗng (`''`).

## 4.3 CI readiness
`ci-readiness.yml` đã được cập nhật chạy theo chế độ `R2-only strict` bằng cấu hình R2 giả lập để không còn lệch với production.

## 5) Cloudflare R2 CORS checklist
Bucket R2 cần CORS cho browser direct upload:
- Methods: `PUT, GET, HEAD`
- Allowed Origins: chỉ domain FE thật (prod/staging/local), không dùng `*` ở production
- Allowed Headers: tối thiểu `content-type, x-amz-date, x-amz-content-sha256, authorization, origin`
- Expose Headers: `etag, x-amz-request-id` (khuyến nghị)

## 6) Post-deploy verification checklist

## 6.1 Avatar
1. Chọn ảnh > thấy progress upload.
2. Upload thành công -> gọi confirm -> avatar DB cập nhật.
3. Avatar cũ trên R2 bị xóa (best effort).

## 6.2 Community post/comment image
1. Attach ảnh trong post/comment -> placeholder markdown xuất hiện.
2. Upload + confirm thành công -> placeholder được thay bằng URL thật.
3. Fail upload/confirm -> rollback placeholder + báo lỗi.

## 6.3 Chat image/voice
1. Presign theo `mediaKind=image|voice`.
2. PUT trực tiếp R2 có progress.
3. Gửi message kèm `url + objectKey + uploadToken`.
4. BE consume `uploadToken` one-time (token không reuse).

## 7) Rủi ro rollout thường gặp
- `OBJECTSTORAGE_PROVIDER` không phải `R2` -> backend fail start do R2-only strict.
- Thiếu 1 trong 5 biến `R2_*` bắt buộc -> presign/confirm lỗi.
- CORS R2 sai origin/header -> browser PUT fail dù presign thành công.
- Quên cập nhật `PROD_DOTENV_*` sau khi đổi `.env` -> pipeline deploy dùng config cũ.

## 8) Khuyến nghị vận hành
- Sau mỗi lần đổi `.env`, cập nhật lại `PROD_DOTENV_PLAIN` hoặc `PROD_DOTENV_B64` ngay.
- Giữ TTL presign ngắn (10 phút mặc định).
- Theo dõi log cleanup job để phát hiện orphan media tăng bất thường.
