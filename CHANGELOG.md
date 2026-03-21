# BẢN GHI LỊCH SỬ THAY ĐỔI (CHANGELOG)

Tài liệu này dùng để theo dõi tất cả các chức năng mới, nâng cấp, và sửa lỗi trên hệ thống TarotNowAI.

## [Nguyên nhân & Thay đổi] - 20/03/2026

### Cập nhật Dịch vụ Gửi OTP & Email của Hệ thống
- **Vấn đề (Issue)**: Nhận được báo cáo từ người dùng là mã OTP gửi về Email không hoạt động.
- **Nguyên nhân cốt lõi (Root Cause)**: Backend đang được cấu hình sử dụng `MockEmailSender` (Trình gửi mail giả lập cho Dev) nhằm tiết kiệm chi phí gọi API gửi mail khi đang lập trình thử nghiệm. `MockEmailSender` chỉ có chức năng in mã OTP xuống Terminal máy chủ.
- **Giải pháp xử lý (Resolution)**:
  - Cài đặt thêm thư viện chính thống `AWSSDK.SimpleEmail` Version `3.7.400.41` vào Project `TarotNow.Infrastructure`.
  - Phát triển tính năng `AwsSesEmailSender.cs` kết nối trực tiếp với AWS Cloud để quản lý và gửi Email.
  - Cấu hình thông số AWS cho hệ thống tại bộ nhớ lõi `appsettings.json` bằng object `AwsSes` (Bao gồm AccessKey, SecretKey, Region, và SenderEmail).
  - Gắn lại Dependency Injection (`services.AddScoped<IEmailSender, AwsSesEmailSender>()`) để toàn bộ hệ thống (Kể cả chức năng cấp OTP, đặt lại mật khẩu) tự động nhận diện và gửi thư qua Amazon.

### Các tệp ảnh hưởng cụ thể
1. `Backend/src/TarotNow.Infrastructure/TarotNow.Infrastructure.csproj`
2. `Backend/src/TarotNow.Infrastructure/Services/AwsSesEmailSender.cs` (Mới)
3. `Backend/src/TarotNow.Infrastructure/DependencyInjection.cs`
4. `Backend/src/TarotNow.Api/appsettings.json`

---

## [Chuyển đổi Architecture] - 20/03/2026

### Nâng cấp Dịch vụ Gửi OTP từ Amazon SES sang Cấu trúc linh hoạt Gmail SMTP
- **Nhu cầu (Requirement)**: Chuyển đổi công nghệ gửi email để không phụ thuộc vào nền tảng trả phí Amazon SES, hướng đến sử dụng cấu hình Gmail SMTP (Hoặc Outlook SMTP) hoàn toàn miễn phí và linh động.
- **Giải pháp chuyển tiếp (Migration)**: 
  - Gỡ bỏ package `AWSSDK.SimpleEmail` và thay bằng hệ sinh thái phổ quát `MailKit` version 4.4.0.
  - Viết lại class `SmtpEmailSender.cs` kết nối trực tiếp đến giao thức TLS của Google trên cổng 587 bằng thư viện MimeKit và SmtpClient. 
  - Thiết lập lại cấu hình DI và mở block `"Smtp"` trong file môi trường API để cấp phép tài khoản gửi thư thật.

---
