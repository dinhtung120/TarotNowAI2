


async function run() {
    // Đăng nhập để lấy access token trước khi gọi API cần xác thực.
    const loginRes = await fetch("http://localhost:5037/api/v1/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ emailOrUsername: "testuser4@tarotnow.com", password: "Password123!" })
    });
    const loginText = await loginRes.text();
    console.log("Login:", loginRes.status, loginText);

    // Dừng sớm nếu login thất bại để tránh test giả âm tính ở bước tiếp theo.
    if (loginRes.status !== 200) return;

    // Parse token từ response để gắn vào header Authorization.
    const token = JSON.parse(loginText).accessToken;

    // Gọi endpoint init reading để kiểm tra luồng API cơ bản.
    const initRes = await fetch("http://localhost:5037/api/v1/reading/init", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify({ spreadType: "daily_1" })
    });
    const text = await initRes.text();
    console.log("Init:", initRes.status, text);
}
run();
