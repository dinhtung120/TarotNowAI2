/*
 * FILE: test-reveal.js
 * MỤC ĐÍCH: Script test LUỒNG ĐẦY ĐỦ — Login → Init → Reveal (lật bài xem kết quả).
 *   Chạy: node test-reveal.js
 *
 *   LUỒNG TEST (3 bước):
 *   1. Đăng nhập → lấy accessToken
 *   2. Init phiên spread_3 (trả phí) → lấy sessionId
 *   3. Reveal: gửi sessionId → server xào bài + gán lá + trả kết quả
 *
 *   ĐÂY LÀ SCRIPT TEST ĐẦY ĐỦ NHẤT:
 *   → test-api.js: chỉ test init (daily miễn phí)
 *   → test-paid.js: chỉ test init spread_3 (trả phí)
 *   → test-reveal.js: test TOÀN BỘ luồng init + reveal (xào bài + gán kết quả)
 *
 *   KẾT QUẢ MONG ĐỢI:
 *   → Reveal Status: 200
 *   → Response: JSON chứa cards đã rút (drawn_cards), spread info, v.v.
 *
 *   LƯU Ý: User cần có đủ Gold/Diamond + server đang chạy.
 */

async function run() {
    // ===== BƯỚC 1: Đăng nhập =====
    const loginRes = await fetch("http://localhost:5037/api/v1/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ emailOrUsername: "testuser4@tarotnow.com", password: "Password123!" })
    });
    const loginText = await loginRes.text();
    const token = JSON.parse(loginText).accessToken;

    // ===== BƯỚC 2: Init phiên spread_3 (trả phí) =====
    const initRes = await fetch("http://localhost:5037/api/v1/reading/init", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify({ spreadType: "spread_3" })
    });
    const initData = await initRes.json();
    // Kiểm tra init thành công — phải có sessionId
    if (!initData.sessionId) {
        console.log("Fail INIT:", initData);
        return;
    }
    const sessionId = initData.sessionId;

    // ===== BƯỚC 3: Reveal — lật bài xem kết quả =====
    const revealRes = await fetch("http://localhost:5037/api/v1/reading/reveal", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify({ sessionId: sessionId }) // Gửi sessionId từ bước 2
    });
    const text = await revealRes.text();
    console.log("Reveal Status:", revealRes.status);
    console.log("Response:", text); // JSON chứa drawn_cards + spread info
}
run();
