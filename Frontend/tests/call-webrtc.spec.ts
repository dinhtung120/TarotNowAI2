import { expect, test, type Page } from '@playwright/test';

const BASE_URL = process.env.QA_BASE_URL || 'http://127.0.0.1:3100';

// Config WebRTC fake media for testing
test.use({
  launchOptions: {
    args: [
      '--use-fake-ui-for-media-stream',
      '--use-fake-device-for-media-stream',
    ],
  },
  permissions: ['camera', 'microphone'],
});

test.describe('WebRTC Call E2E (Smoke)', () => {
  test.setTimeout(120_000);

  const goto = (page: Page, path: string) =>
    page.goto(`${BASE_URL}${path}`, { waitUntil: 'domcontentloaded', timeout: 60_000 });

  test('Call UI components are protected by auth', async ({ page }) => {
    // Nếu chưa đăng nhập, vào trang chat chứa CallProvider sẽ bị đẩy ra
    await goto(page, '/vi/chat/12345-fake-id');
    await expect(page).toHaveURL(/\/vi\/login/);
  });

  // Ghi chú: E2E Test chuyên sâu 2 bên nghe/gọi yêu cầu:
  // 1. Script tự động đăng nhập tạo 2 user context
  // 2. Browser 1 bấm gọi => SignalR Relay
  // 3. Browser 2 chờ thấy locator `text=Cuộc gọi đến...`
  // 4. Bấm Accept => Cả 2 hiện `video` element (MediaStream fake)
  
  // Dưới đây là khung test giả lập (Mocked UI) nếu Frontend cho phép truy cập Store qua window.
  // Trong thực tế, Playwright sẽ dùng 2 Context (Caller & Receiver) để test e2e real-time.
  
  test('Two-way WebRTC Call Simulation (Placeholder for Authenticated Flow)', async ({ browser }) => {
    // Context 1: Caller
    const callerContext = await browser.newContext({
      permissions: ['camera', 'microphone'],
    });
    // Context 2: Receiver
    const receiverContext = await browser.newContext({
      permissions: ['camera', 'microphone'],
    });

    // const callerPage = await callerContext.newPage();
    // const receiverPage = await receiverContext.newPage();

    // Bước này cần auth cho cả 2 page để vào chung /chat/:conversationId
    // await callerPage.goto(`${BASE_URL}/vi/chat/test-room`);
    // await receiverPage.goto(`${BASE_URL}/vi/chat/test-room`);

    // await callerPage.getByTitle('Gọi Video').click();
    
    // await expect(receiverPage.locator('text=Cuộc gọi đến...')).toBeVisible();
    // await receiverPage.getByRole('button', { name: 'Accept' }).click();

    // // Cả hai sẽ nhảy sang giao diện Connected (Cuộc Sẻ Chia) với 2 thẻ video
    // await expect(callerPage.locator('text=Cuộc Sẻ Chia')).toBeVisible();
    // await expect(receiverPage.locator('text=Cuộc Sẻ Chia')).toBeVisible();

    // // Nhấn kết thúc
    // await callerPage.getByRole('button', { name: 'End Call' }).click();
    
    // await expect(receiverPage.locator('text=Cuộc Sẻ Chia')).toBeHidden();

    await callerContext.close();
    await receiverContext.close();
  });
});
