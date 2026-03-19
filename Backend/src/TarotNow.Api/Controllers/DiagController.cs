/*
 * ===================================================================
 * FILE: DiagController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller CHẨN ĐOÁN (Diagnostics) - chỉ dùng trong MÔI TRƯỜNG PHÁT TRIỂN (Development).
 *   Cung cấp các công cụ để developer debug và setup hệ thống:
 *   1. Wipe: Xóa dữ liệu (hiện tại đã disabled)
 *   2. Seed Admin: Tự động tạo tài khoản Super Admin
 *   3. Stats: Xem thống kê dữ liệu MongoDB
 *
 * BẢO MẬT NHIỀU TẦNG:
 *   Tầng 1: [Authorize(Roles = "admin")] → chỉ admin mới gọi được
 *   Tầng 2: RejectIfNotDevelopment() → chỉ chạy trong môi trường Development
 *   Tầng 3: [ApiExplorerSettings(IgnoreApi = true)] → ẩn khỏi Swagger/OpenAPI
 *   
 *   Ngay cả khi hacker biết URL, họ cần:
 *   - Có JWT token admin (tầng 1)
 *   - Server phải đang chạy ở Development mode (tầng 2)
 *   Trong Production, tất cả endpoint đều trả 404 Not Found.
 *
 * CẢNH BÁO:
 *   KHÔNG BAO GIỜ để endpoint này hoạt động trên production server.
 *   Nó có quyền tạo admin, xóa dữ liệu - rất nguy hiểm nếu bị lạm dụng.
 * ===================================================================
 */

using Microsoft.AspNetCore.Authorization; // [Authorize] kiểm soát quyền
using Microsoft.AspNetCore.Mvc;           // API controller base
using Microsoft.EntityFrameworkCore;       // Entity Framework - truy vấn SQL database
using MongoDB.Bson;                       // Kiểu dữ liệu BSON cho MongoDB queries
using MongoDB.Driver;                     // MongoDB driver - truy vấn MongoDB

using TarotNow.Application.Interfaces;   // Interface IPasswordHasher
using TarotNow.Infrastructure.Persistence; // DbContext (SQL và MongoDB)

namespace TarotNow.Api.Controllers;

/*
 * [ApiExplorerSettings(IgnoreApi = true)]:
 *   ẩn tất cả endpoint trong controller này khỏi Swagger UI.
 *   Swagger là trang web tự động sinh tài liệu API (thường ở /swagger).
 *   Ẩn đi vì đây là endpoint nội bộ, không muốn ai biết đến.
 */
[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "admin")]
[ApiExplorerSettings(IgnoreApi = true)] // Ẩn khỏi Swagger - endpoint bí mật
public class DiagController : ControllerBase
{
    /*
     * _dbContext: EF Core DbContext - truy cập trực tiếp SQL database (PostgreSQL).
     *   ⚠️ NGOẠI LỆ: Controller này inject DbContext trực tiếp (vi phạm Clean Architecture)
     *   nhưng CHẤP NHẬN ĐƯỢC vì đây là tool nội bộ, không phải business logic.
     *
     * _passwordHasher: Service băm mật khẩu (Argon2id).
     *   Dùng để tạo hash cho mật khẩu admin khi seed.
     *
     * _environment: Thông tin môi trường (Development/Production).
     *   Dùng để kiểm tra và chặn endpoint nếu không phải Development.
     *
     * _configuration: Đọc cấu hình từ appsettings.json.
     *   Dùng để lấy email/password cho admin seed.
     *
     * _logger: Ghi log để theo dõi và debug.
     */
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DiagController> _logger;

    public DiagController(
        ApplicationDbContext dbContext,
        IPasswordHasher passwordHasher,
        IWebHostEnvironment environment,
        IConfiguration configuration,
        ILogger<DiagController> logger)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _environment = environment;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Guard method (hàm bảo vệ): kiểm tra môi trường hiện tại.
    /// Nếu KHÔNG phải Development → trả 404 Not Found (giả vờ endpoint không tồn tại).
    /// Nếu là Development → trả null (cho phép tiếp tục xử lý).
    /// 
    /// "Guard clause" (mệnh đề bảo vệ) là pattern thường dùng:
    ///   thay vì if-else lồng nhau, kiểm tra điều kiện sai → return sớm.
    /// </summary>
    private IActionResult? RejectIfNotDevelopment()
    {
        // IsDevelopment(): kiểm tra biến môi trường ASPNETCORE_ENVIRONMENT = "Development"
        if (_environment.IsDevelopment()) return null; // null = OK, tiếp tục
        return NotFound(); // Giả vờ endpoint không tồn tại trong Production
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Diag/wipe
    /// MỤC ĐÍCH: Xóa toàn bộ dữ liệu (database reset).
    /// HIỆN TRẠNG: Đã bị vô hiệu hóa (disabled) để tránh tai nạn.
    /// </summary>
    [HttpPost("wipe")]
    public IActionResult Wipe()
    {
        // Kiểm tra môi trường Development
        var guard = RejectIfNotDevelopment();
        if (guard != null) return guard; // Không phải Dev → 404

        // Trả notice: tính năng đã bị tắt
        return Ok(new { message = "Wipe endpoint is disabled by default." });
    }

    /// <summary>
    /// ENDPOINT: POST /api/v1/Diag/seed-admin
    /// MỤC ĐÍCH: Tạo hoặc cập nhật tài khoản Super Admin.
    ///
    /// KHI NÀO DÙNG?
    ///   - Lần đầu setup hệ thống (database mới, chưa có admin nào)
    ///   - Quên mật khẩu admin (dùng endpoint này để reset)
    ///   - Chuyển quyền admin sang email khác
    ///
    /// CÁCH CẤU HÌNH:
    ///   Thêm vào appsettings.Development.json:
    ///   {
    ///     "Diagnostics": {
    ///       "SeedAdmin": {
    ///         "Email": "admin@tarot.com",
    ///         "Username": "superadmin",
    ///         "Password": "StrongP@ssw0rd123"
    ///       }
    ///     }
    ///   }
    /// </summary>
    [HttpPost("seed-admin")]
    public async Task<IActionResult> SeedAdmin()
    {
        // Guard: chỉ cho phép trong Development
        var guard = RejectIfNotDevelopment();
        if (guard != null) return guard;

        try 
        {
            // ========================================
            // BƯỚC 1: ĐỌC CẤU HÌNH ADMIN TỪ APPSETTINGS
            // ========================================

            /*
             * Đọc email, username, password admin từ file cấu hình.
             * Trim(): xóa khoảng trắng thừa đầu/cuối (tránh lỗi khi copy-paste).
             */
            var adminEmail = _configuration["Diagnostics:SeedAdmin:Email"]?.Trim();
            var adminUsername = _configuration["Diagnostics:SeedAdmin:Username"]?.Trim();
            var adminPassword = _configuration["Diagnostics:SeedAdmin:Password"];

            // Kiểm tra: tất cả thông tin phải có và password phải đủ mạnh (≥12 ký tự)
            if (string.IsNullOrWhiteSpace(adminEmail) ||
                string.IsNullOrWhiteSpace(adminUsername) ||
                string.IsNullOrWhiteSpace(adminPassword) ||
                adminPassword.Length < 12) // Yêu cầu password admin ít nhất 12 ký tự
            {
                return BadRequest(new
                {
                    message = "Missing diagnostics seed admin config. Set Diagnostics:SeedAdmin:{Email,Username,Password} with strong password."
                });
            }

            // ========================================
            // BƯỚC 2: KIỂM TRA VÀ TẠO/CẬP NHẬT ADMIN
            // ========================================

            /*
             * Tìm user có email trùng trong database PostgreSQL.
             * FirstOrDefaultAsync: trả về user đầu tiên tìm thấy, hoặc null nếu không có.
             */
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);

            // Băm mật khẩu bằng Argon2id (thuật toán an toàn nhất hiện tại)
            // KHÔNG LƯU mật khẩu gốc vào database → chỉ lưu hash
            var passwordHash = _passwordHasher.HashPassword(adminPassword);

            bool isNew = false; // Cờ đánh dấu: tạo mới hay cập nhật

            if (user == null)
            {
                // --------- TRƯỜNG HỢP 1: CHƯA CÓ ADMIN → TẠO MỚI ---------
                isNew = true;

                /*
                 * Tạo đối tượng User mới với thông tin admin:
                 * - email: email admin
                 * - username: tên đăng nhập
                 * - passwordHash: mật khẩu đã được băm
                 * - displayName: "Super Admin"
                 * - dateOfBirth: ngày giả định (không quan trọng cho admin)
                 * - true: flag isEmailVerified = true (bỏ qua verify email)
                 */
                user = new TarotNow.Domain.Entities.User(
                    adminEmail, 
                    adminUsername,
                    passwordHash, 
                    "Super Admin", 
                    new DateTime(1985, 5, 5).ToUniversalTime(), // Ngày sinh mặc định
                    true // Email đã xác thực
                );

                // Kích hoạt tài khoản (trạng thái Active)
                user.Activate();

                // Nâng quyền lên admin
                user.PromoteToAdmin();
                
                // Thêm user mới vào database (chưa lưu, chờ SaveChanges)
                await _dbContext.Users.AddAsync(user);
            }
            else
            {
                // --------- TRƯỜNG HỢP 2: ĐÃ CÓ → CẬP NHẬT ---------
                // Đảm bảo user có quyền admin và trạng thái active
                user.PromoteToAdmin();
                user.Activate();

                // Cập nhật mật khẩu mới (hữu ích khi quên mật khẩu)
                user.UpdatePassword(passwordHash);
            }

            // Lưu tất cả thay đổi vào database PostgreSQL
            await _dbContext.SaveChangesAsync();

            // Trả về thông tin admin đã tạo/cập nhật
            return Ok(new { 
                Message = isNew ? "SuperAdmin created" : "SuperAdmin updated", 
                Email = adminEmail, 
                Username = adminUsername
                // KHÔNG trả về password hay hash → bảo mật
            });
        }
        catch (Exception ex)
        {
            // Ghi log lỗi và trả 500
            _logger.LogError(ex, "Failed to seed admin account");
            return StatusCode(500, new { message = "Failed to seed admin account." });
        }
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/Diag/stats
    /// MỤC ĐÍCH: Xem thống kê dữ liệu MongoDB để kiểm tra hệ thống hoạt động đúng.
    ///
    /// THÔNG TIN TRẢ VỀ:
    ///   - Tổng số reading sessions trong MongoDB
    ///   - Số sessions của một test user cụ thể
    ///   - 5 document mẫu (sample) để kiểm tra cấu trúc dữ liệu
    ///   
    /// [FromServices]: Inject MongoDbContext trực tiếp vào method (thay vì constructor).
    ///   Kỹ thuật này gọi là "method injection" - dùng khi dependency chỉ cần ở một method.
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats([FromServices] TarotNow.Infrastructure.Persistence.MongoDbContext mongoContext)
    {
        // Guard: chỉ cho phép trong Development
        var guard = RejectIfNotDevelopment();
        if (guard != null) return guard;

        try
        {
            /*
             * CountDocumentsAsync(new BsonDocument()): đếm TẤT CẢ documents trong collection.
             * BsonDocument() rỗng = không có điều kiện lọc = lấy hết.
             * BSON là format nhị phân của JSON, dùng trong MongoDB.
             */
            var totalSessions = await mongoContext.ReadingSessions.CountDocumentsAsync(new BsonDocument());

            /*
             * Đếm sessions của một test user cụ thể (hardcoded ID).
             * Dùng để kiểm tra xem dữ liệu có được phân bổ đúng user không.
             * 
             * Builders<T>.Filter.Eq: tạo bộ lọc "bằng" (equals).
             * Eq(r => r.UserId, testUserId): tìm documents có UserId = testUserId.
             */
            var testUserId = "c6f6ca4e-042d-44c8-8812-bdce1b4b1563";
            var testUserSessions = await mongoContext.ReadingSessions.CountDocumentsAsync(
                Builders<TarotNow.Infrastructure.Persistence.MongoDocuments.ReadingSessionDocument>.Filter.Eq(r => r.UserId, testUserId)
            );

            /*
             * Lấy 5 documents mẫu (sample) từ collection.
             * Find(new BsonDocument()): tìm tất cả (không lọc).
             * Limit(5): chỉ lấy 5 document đầu tiên.
             * ToListAsync(): chuyển kết quả thành List<T>.
             */
            var sampleDocs = await mongoContext.ReadingSessions
                .Find(new BsonDocument())
                .Limit(5)
                .ToListAsync();

            // Chuyển đổi documents thành JSON string để dễ đọc
            // ToJson(): method của BsonDocument, serialize thành JSON string
            var rawJsonSamples = sampleDocs.Select(d => d.ToJson()).ToList();

            // Trả về thống kê
            return Ok(new { 
                TotalSessionsInMongo = totalSessions,    // Tổng sessions
                TestUserSessions = testUserSessions,     // Sessions của test user
                SampleDataRaw = rawJsonSamples           // 5 document mẫu
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch diagnostics stats");
            return StatusCode(500, new { message = "Failed to fetch diagnostics stats." });
        }
    }
}
