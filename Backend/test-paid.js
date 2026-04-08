


async function run() {
  // Đăng nhập để lấy JWT dùng cho luồng reading trả phí.
  const loginRes = await fetch("http://localhost:5037/api/v1/auth/login", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ emailOrUsername: "testuser4@tarotnow.com", password: "Password123!" })
  });
  const loginText = await loginRes.text();
  const token = JSON.parse(loginText).accessToken;

  // Gọi init reading spread_3 để kiểm tra nhánh có trừ phí.
  const initRes = await fetch("http://localhost:5037/api/v1/reading/init", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${token}`
    },
    body: JSON.stringify({ spreadType: "spread_3" })
  });
  const text = await initRes.text();
  console.log("Init:", initRes.status, text);
}
run();
