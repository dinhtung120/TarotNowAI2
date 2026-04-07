import { expect, test, type Page } from '@playwright/test';

const BASE_URL = process.env.QA_BASE_URL || 'http://127.0.0.1:3100';


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
    
    await goto(page, '/vi/chat/12345-fake-id');
    await expect(page).toHaveURL(/\/vi\/login/);
  });

  
  
  
  
  
  
  
  
  
  test('Two-way WebRTC Call Simulation (Placeholder for Authenticated Flow)', async ({ browser }) => {
    
    const callerContext = await browser.newContext({
      permissions: ['camera', 'microphone'],
    });
    
    const receiverContext = await browser.newContext({
      permissions: ['camera', 'microphone'],
    });

    
    

    
    
    

    
    
    
    

    
    
    

    
    
    
    

    await callerContext.close();
    await receiverContext.close();
  });
});
