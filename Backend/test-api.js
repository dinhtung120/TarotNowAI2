/*
 * FILE: test-api.js
 * MỤC ĐÍCH: Script test nhanh API endpoints từ terminal (Node.js).
 *   Chạy: node test-api.js
 *
 *   LUỒNG TEST:
 *   1. Đăng nhập (POST /api/v1/auth/login) → lấy accessToken
 *   2. Tạo phiên đọc bài miễn phí (POST /api/v1/reading/init) → daily_1 (rút 1 lá/ngày)
 *
 *   KHI NÀO DÙNG?
 *   → Dev test nhanh API mà không cần Postman/Swagger
 *   → Verify luồng auth + reading init hoạt động end-to-end
 *   → Debug: log status + response body ra console
 *
 *   LƯU Ý: Đây là script test thủ công, KHÔNG phải automated test.
 *   Cần server đang chạy tại localhost:5037.
 */

// Hàm chính: đăng nhập → tạo phiên đọc bài
async function run() {
    // Bước 1: Đăng nhập với tài khoản test
    const loginRes = await fetch("http://localhost:5037/api/v1/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ emailOrUsername: "testuser4@tarotnow.com", password: "Password123!" })
    });
    const loginText = await loginRes.text();
    console.log("Login:", loginRes.status, loginText);

    // Dừng nếu đăng nhập thất bại
    if (loginRes.status !== 200) return;

    // Trích xuất access token từ response
    const token = JSON.parse(loginText).accessToken;

    // Bước 2: Tạo phiên đọc bài daily (miễn phí, 1 lá/ngày)
    const initRes = await fetch("http://localhost:5037/api/v1/reading/init", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}` // JWT Bearer auth
        },
        body: JSON.stringify({ spreadType: "daily_1" }) // Trải bài: rút 1 lá hàng ngày
    });
    const text = await initRes.text();
    console.log("Init:", initRes.status, text);
}
run();
