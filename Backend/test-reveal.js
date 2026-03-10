async function run() {
    // 1. Login
    const loginRes = await fetch("http://localhost:5037/api/v1/auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ emailOrUsername: "testuser4@tarotnow.com", password: "Password123!" })
    });
    const loginText = await loginRes.text();
    const token = JSON.parse(loginText).accessToken;

    // 2. Init
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
        console.log("Fail INIT:", initData);
        return;
    }
    const sessionId = initData.sessionId;

    // 3. Reveal
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
