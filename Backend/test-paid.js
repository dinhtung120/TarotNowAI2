/*
 * FILE: test-paid.js
 * MỤC ĐÍCH: Script test luồng THANH TOÁN — tạo phiên đọc bài TRẢ PHÍ (spread_3).
 *   Chạy: node test-paid.js
 *
 *   LUỒNG TEST:
 *   1. Đăng nhập → lấy accessToken
 *   2. Tạo phiên đọc bài spread_3 (trải 3 lá — tốn Gold/Diamond)
 *
 *   KHÁC VỚI test-api.js:
 *   → test-api.js dùng daily_1 (miễn phí)
 *   → test-paid.js dùng spread_3 (trả phí) → kiểm tra luồng freeze + debit
 *   → User phải có đủ Gold/Diamond trong ví, nếu không → lỗi 402 Payment Required
 *
 *   LƯU Ý: Cần server đang chạy + User có số dư đủ.
 */

// Hàm chính: đăng nhập → tạo phiên trải BÀI 3 lá (trả phí)
async function run() {
  // Bước 1: Đăng nhập
  const loginRes = await fetch("http://localhost:5037/api/v1/auth/login", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ emailOrUsername: "testuser4@tarotnow.com", password: "Password123!" })
  });
  const loginText = await loginRes.text();
  const token = JSON.parse(loginText).accessToken;

  // Bước 2: Tạo phiên trải 3 lá (trả phí — spread_3 tốn Gold/Diamond)
  const initRes = await fetch("http://localhost:5037/api/v1/reading/init", {
    method: "POST",
    headers: { 
      "Content-Type": "application/json",
      "Authorization": `Bearer ${token}`
    },
    body: JSON.stringify({ spreadType: "spread_3" }) // Trải 3 lá → yêu cầu thanh toán
  });
  const text = await initRes.text();
  console.log("Init:", initRes.status, text);
}
run();
