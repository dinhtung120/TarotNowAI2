import { test, expect } from '@playwright/test';

test('redirects private route to login when no auth cookie', async ({ page }) => {
  const baseURL = process.env.QA_BASE_URL || 'http://127.0.0.1:3100';
  await page.goto(`${baseURL}/vi/chat`);
  await expect(page).toHaveURL(/\/vi\/login/);
});

test('reading stream proxy rejects when unauthenticated', async ({ request }) => {
  const baseURL = process.env.QA_BASE_URL || 'http://127.0.0.1:3100';
  const response = await request.get(
    `${baseURL}/vi/api/reading/sessions/00000000-0000-0000-0000-000000000000/stream`
  );
  expect(response.status()).toBe(401);
});
