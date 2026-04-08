

async function run() {
    // Đăng nhập để lấy token gọi chuỗi API init -> reveal.
    const loginRes = await fetch("http://localhost:5037/api/v1/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ emailOrUsername: "testuser4@tarotnow.com", password: "Password123!" })
    });
    const loginText = await loginRes.text();
    const token = JSON.parse(loginText).accessToken;

    // Tạo session reading trước khi gọi reveal.
    const initRes = await fetch("http://localhost:5037/api/v1/reading/init", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify({ spreadType: "spread_3" })
    });
    const initData = await initRes.json();

    if (!initData.sessionId) {
        // Edge case: init không trả sessionId thì dừng để tránh gọi reveal lỗi giả.
        console.log("Fail INIT:", initData);
        return;
    }
    const sessionId = initData.sessionId;

    // Reveal kết quả cho session đã khởi tạo để kiểm tra end-to-end reading flow.
    const revealRes = await fetch("http://localhost:5037/api/v1/reading/reveal", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify({ sessionId: sessionId })
    });
    const text = await revealRes.text();
    console.log("Reveal Status:", revealRes.status);
    console.log("Response:", text);
}
run();
