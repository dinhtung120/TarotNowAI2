import { expect, test, type Page } from '@playwright/test';

const BASE_URL = process.env.QA_BASE_URL || 'http://127.0.0.1:3100';

test.describe('smoke flows', () => {
 test.describe.configure({ mode: 'serial' });
 test.setTimeout(120_000);

 const goto = (page: Page, path: string) =>
  page.goto(`${BASE_URL}${path}`, { waitUntil: 'domcontentloaded', timeout: 60_000 });

 test('auth flow: login/register/verify pages are reachable', async ({ page }) => {
  for (const route of ['/vi/login', '/vi/register', '/vi/verify-email']) {
   await goto(page, route);
   await expect(page).toHaveURL(new RegExp(`${route}$`));
  }
 });

 test('reading flow: setup route redirects to login without auth', async ({ page }) => {
  await goto(page, '/vi/reading');
  await expect(page).toHaveURL(/\/vi\/login/);
 });

 test('chat flow: private route redirects to login without auth', async ({ page }) => {
  await goto(page, '/vi/chat');
  await expect(page).toHaveURL(/\/vi\/login/);
 });

 test('wallet flow: private route redirects to login without auth', async ({ page }) => {
  await goto(page, '/vi/wallet');
  await expect(page).toHaveURL(/\/vi\/login/);
 });

 test('admin flow: private route redirects to login without auth', async ({ page }) => {
  await goto(page, '/vi/admin');
  await expect(page).toHaveURL(/\/vi\/login/);
 });
});
