# Hướng dẫn cấu trúc thư mục dự án TarotNow

Tài liệu này giải thích chi tiết mục đích của từng thư mục trong dự án TarotNow, được thiết kế theo chuẩn Clean Architecture. Người phát triển (Backend, Frontend, Mobile) cần đọc kỹ để biết nơi đặt code cho phù hợp.

---

## 1. Backend (.NET 10) - Clean Architecture

Kiến trúc Backend chia thành 4 lớp chính để đảm bảo tính độc lập, dễ kiểm thử và dễ bảo trì. Nguyên tắc cốt lõi: Lớp bên ngoài phụ thuộc vào lớp bên trong, lớp trong cùng (Domain) không phụ thuộc vào bất kỳ lớp nào khác.

### 1.1. `Backend/src/TarotNow.Domain/` (Lớp Cốt Lõi)
Lớp này chứa các khái niệm nghiệp vụ cốt lõi của ứng dụng. **KHÔNG** chứa bất kỳ reference nào đến các framework, database (như Entity Framework) hay công nghệ bên ngoài.
*   **`Entities/`**: Các thực thể kinh doanh đại diện cho state của hệ thống (Ví dụ: `User`, `TarotCard`, `ReadingSession`). Chứa thuộc tính và các phương thức thay đổi trạng thái nội bộ.
*   **`Enums/`**: Các hằng số liệt kê được sử dụng toàn cục (Ví dụ: `CardSuit`, `ReadingStatus`, `UserRole`).
*   **`Events/`**: Xử lý Domain Events. Sử dụng khi một thay đổi trong Domain cần thông báo cho các phần khác của hệ thống mà không tạo ra sự phụ thuộc trực tiếp.
*   **`Exceptions/`**: Chứa các Exception đặc thù của business rule (Ví dụ: `InsufficientCreditsException`).
*   **`Interfaces/`**: Hợp đồng (Contracts) mà Domain yêu cầu, ví dụ như `IRepository<T>` (sẽ được implement ở Infrastructure).
*   **`ValueObjects/`**: Đối tượng không có danh tính, được định nghĩa thông qua các thuộc tính của chúng (Ví dụ: `EmailAddress`, `Money`).

### 1.2. `Backend/src/TarotNow.Application/` (Lớp Ứng Dụng)
Lớp này điều phối các quy tắc nghiệp vụ. Nó định nghĩa *hệ thống có thể làm gì* (Use Cases). Phụ thuộc vào Domain, nhưng độc lập với UI, Database, và các API bên ngoài. Thường áp dụng pattern CQRS (Command Query Responsibility Segregation) và MediatR.
*   **`Features/`**: Chứa phần lớn code logic. Thường phân chia theo chức năng (Ví dụ: `Features/Users/Commands/CreateUser/`, `Features/Readings/Queries/GetReadingById/`). Mỗi Feature sẽ chứa Command/Query, Handler, DTO liên quan.
*   **`Contracts/`**: Các interface để kết nối với các dịch vụ bên ngoài (như EmailService, PaymentGateway) mà Application cần sử dụng nhưng không trực tiếp implement.
*   **`DTOs/`**: Data Transfer Objects. Các object dùng để truyền tải dữ liệu giữa các lớp, đặc biệt là từ/đến Presentation layer.
*   **`Exceptions/`**: Application-level exceptions (Ví dụ: `ValidationException`, `NotFoundException`).
*   **`Mappings/`**: Cấu hình map dữ liệu (như AutoMapper profiles) giữa Entities và DTOs.
*   **`Behaviors/`**: Các Pipeline Behaviors của MediatR (như Logging, Validation, Performance tracking) chạy trước/sau các Handlers.
*   **`Validators/`**: Business rule validation (như FluentValidation) cho các Commands/Queries.
*   **`Messaging/`**: Xử lý việc gửi/nhận message hoặc event ra các hệ thống bên ngoài (như RabbitMQ, Kafka) nếu có.

### 1.3. `Backend/src/TarotNow.Infrastructure/` (Lớp Cơ Sở Hạ Tầng)
Chứa các implement kỹ thuật để tương tác với thế giới bên ngoài. Đây là nơi duy nhất reference đến Entity Framework, thư viện gửi email, caching, hệ thống file cụ thể. Phụ thuộc vào Application và Domain.
*   **`Persistence/`**: Mọi thứ liên quan đến Database (PostgreSQL).
    *   `Repositories/`: Chứa các class implement những repository interface đã định nghĩa trong Domain/Application.
    *   `Configurations/`: Nơi chứa Fluent API conventions cho các Entity (ví dụ cấu hình max length, index).
*   **`Authentication/`**: Logic liên quan đến xác thực và ủy quyền (như tạo JWT token, xác thực OAuth).
*   **`BackgroundJobs/`**: Xử lý các tác vụ nền, chạy theo định kỳ (ví dụ như Quartz.NET, Hangfire).
*   **`Caching/`**: Implement cơ chế cache (như Redis, In-Memory).
*   **`Services/`**: Implement các interface dịch vụ bên thứ 3 đã định nghĩa ở Application (Ví dụ: Gửi email qua SendGrid, thanh toán qua Stripe, gọi đến AI Model).

### 1.4. `Backend/src/TarotNow.Api/` (Lớp Trình Diễn)
Đây là entry point của ứng dụng (Thường là ASP.NET Core Web API). Nhiệm vụ mỏng nhất có thể: tiếp nhận HTTP request, chuyển cho Application (thường qua MediatR), lấy kết quả tương ứng từ Application và wrap thành HTTP response (200 OK, 400 Bad Request, v.v.). Phụ thuộc vào Application và Infrastructure (để setup DI).
*   **`Controllers/`** (Hoặc Minimal APIs): Định nghĩa các endpoint (routes).
*   **`Middlewares/`**: Nơi đặt các middleware xử lý request pipeline (Ví dụ: Error handling middleware toàn cục).
*   **`Extensions/`**: Chứa extension methods để đăng ký Dependency Injection (Services, Swagger, CORS, v.v.) giúp file Program.cs gọn gàng.
*   **`Filters/`**: Action/Exception/Authorization filters.

### 1.5. `Backend/tests/`
*   Chứa các dự án Unit Tests và Integration Tests, phân bổ theo cấu trúc tương ứng với các lớp trên (Ví dụ: `TarotNow.Domain.UnitTests`, `TarotNow.Application.UnitTests`).

---

## 2. Frontend (Next.js 16 - App Router)

Next.js với mô hình App Router được thiết kế để tối ưu SEO, hỗ trợ React Server Components (RSC) và Client Components.

*   **`src/app/`**: Nơi chứa toàn bộ cấu trúc định tuyến (routes). Mỗi folder tương ứng một phân đoạn URL (path).
    *   **`(auth)/`**: Route group. Thư mục bọc trong dấu ngoặc đơn không ảnh hưởng đến URL. Dùng để gom nhóm các trang như login, register và share chung 1 Layout riêng (không cần Header/Footer bình thường).
    *   **`(dashboard)/`**: Route group cho người dùng đã đăng nhập, share chung Sidebar layout.
    *   **`api/`**: Chứa Next.js Route Handlers. Nếu bạn muốn Next.js đóng vai trò như một proxy server nhỏ che giấu API key hoặc xử lý Webhooks, hãy viết code ở đây (`route.ts`).
*   **`src/components/`**: Các React component tái sử dụng.
    *   **`ui/`**: Các component cơ sở, nhỏ lẻ (Button, Input, Card, Modal, Tooltip). Nơi hiển thị UI thuần túy (Dumb components). Thường sử dụng shadcn/ui.
    *   **`shared/`**, **`layouts/`**: Các phần tử chung, kết cấu phức tạp hơn (Navbar, Footer, Sidebar, Layout container).
*   **`src/hooks/`**: Chứa các Custom React Hooks dùng lại nhiều chỗ (`useAuth()`, `useMediaQuery()`).
*   **`src/lib/`**: Chứa các file tiện ích cấu hình, khởi tạo thư viện bên thứ 3 (prisma client nếu có dùng ở api route, trpc, axios instance interceptors).
*   **`src/services/`**: Code liên quan đến việc call API hệ thống Backend .NET. Đây là nơi chứa logic fetch data (dùng fetch native hoặc thư viện như React Query/SWR).
*   **`src/store/`**: Nếu dùng Global State Management (Zustand, Redux, Jotai).
*   **`src/types/`**: Khai báo các TypeScript interfaces và types dùng chung toàn app (thường map tương ứng với DTO từ backend).
*   **`src/utils/`**: Các helper functions thuần túy (format ngày tháng, format tiền tệ, helper functions, không chứa logic React/JSX).
*   **`public/`**: Thư mục chứa các tài nguyên tĩnh như file hình ảnh, font, icon, robots.txt. Files ở đây được phục vụ trực tiếp.

---

## 3. Mobile App

Cấu trúc phổ biến cho các Framework Mobile hiện đại như React Native / Expo.

*   **`src/assets/`**: Chứa các file tĩnh (hình ảnh, fonts, icons đặc thù của mobile).
*   **`src/constants/`**: Các giá trị hằng số toàn cục (URLs hệ thống, mã lỗi, key lưu Storage).
*   **`src/components/`**: Chứa các UI Components.
    *   **`ui/`**: Core UI (Button custom, text fields phù hợp kích cỡ màn hình thiết bị).
    *   **`shared/`**: Component phức tạp hơn được ghép lại.
*   **`src/hooks/`**: Custom hooks riêng của môi trường mobile (ví dụ hook tracking vị trí GPS, check quyền camera).
*   **`src/navigation/`**: Cấu hình Native Routing (Ví dụ React Navigation). Nơi định nghĩa Stack Navigator (chuyển trang qua lại), Tab Navigator (menu dưới cùng).
*   **`src/screens/`** (Hoặc `pages/`): Nơi tập hợp các màn hình chính (mỗi file thường tương ứng với một Route ở Navigation). Ví dụ: `HomeScreen.tsx`, `ProfileScreen.tsx`.
*   **`src/services/api/`**: Nơi quản lý hàm fetch/post gửi lên API .NET. (Lưu ý xử lý interceptors refresh JWT token vì mobile thường chạy dài ngày).
*   **`src/store/`**: Quản lý state toàn cục (Redux / Zustand). Lưu ý có tính năng persist để lưu state offline dạng AsyncStorage/SecureStore.
*   **`src/theme/`**: Cấu hình các thiết lập giao diện chung gồm Colors, Typography, Spacing để toàn app nhìn đồng nhất.
*   **`src/utils/`**: Helper methods riêng biệt (Scale width/height tuỳ chọn phù hợp thiết bị, helper handle permission thiết bị).

---

## 4. Các thư mục khác ở Root
*   **`docs/`**: Chứa tất cả tài liệu dự án, sơ đồ kiến trúc, tài liệu API, hướng dẫn onboarding.
*   **`scripts/`**: Chứa các file thực thi (bash scripts, powershell) để tự động hóa (Ví dụ: script build, script chạy database tự động, docker-compose).
