import { expect, test, type Page } from '@playwright/test';

const BASE_URL = process.env.QA_BASE_URL || 'http://127.0.0.1:3100';
const PROTECTED_ROUTES = [
 '/vi/reading',
 '/vi/collection',
 '/vi/chat',
 '/vi/gamification',
 '/vi/leaderboard',
 '/vi/gacha',
 '/vi/profile',
 '/vi/wallet',
 '/vi/readers',
 '/vi/community',
 '/vi/premium',
 '/vi/notifications',
 '/vi/reader/apply',
 '/vi/admin',
] as const;

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

 test('auth flow: verify/login query email prefill contracts work', async ({ page }) => {
  await goto(page, '/vi/verify-email?email=query.user@example.com');
  const verifyEmailInput = page.locator('input[name="email"]');
  await expect(verifyEmailInput).toHaveValue('query.user@example.com');
  await expect(verifyEmailInput).toHaveAttribute('readonly', '');

  await goto(page, '/vi/verify-email');
  await expect(verifyEmailInput).not.toHaveAttribute('readonly', '');

  await page.addInitScript(() => {
   window.localStorage.setItem('tarot_remembered_email', 'remembered.user@example.com');
  });
  await goto(page, '/vi/login?email=query.login@example.com');
  const loginIdentityInput = page.locator('input[name="emailOrUsername"]');
  await expect(loginIdentityInput).toHaveValue('query.login@example.com');

  await loginIdentityInput.fill('edited.login@example.com');
  await expect(loginIdentityInput).toHaveValue('edited.login@example.com');
 });

 test('protected routes: unauthenticated user is always redirected to login', async ({ page }) => {
  for (const route of PROTECTED_ROUTES) {
   await goto(page, route);
   await expect(page).toHaveURL(/\/vi\/login/);
  }
 });
});
