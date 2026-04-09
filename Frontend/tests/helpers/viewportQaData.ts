type RouteItem = {
 name: string;
 path: string;
};

type ViewportItem = {
 name: string;
 width: number;
 height: number;
};

export const VIEWPORTS: ViewportItem[] = [
 { name: 'mobile', width: 390, height: 844 },
 { name: 'tablet', width: 834, height: 1194 },
 { name: 'desktop', width: 1440, height: 900 },
];

export const ROUTES: RouteItem[] = [
 { name: 'Home', path: '/vi' },
 { name: 'Legal/TOS', path: '/vi/legal/tos' },
 { name: 'Legal/Privacy', path: '/vi/legal/privacy' },
 { name: 'Legal/AI Disclaimer', path: '/vi/legal/ai-disclaimer' },
 { name: 'Login', path: '/vi/login' },
 { name: 'Register', path: '/vi/register' },
 { name: 'Forgot Password', path: '/vi/forgot-password' },
 { name: 'Reset Password', path: '/vi/reset-password' },
 { name: 'Verify Email', path: '/vi/verify-email' },
 { name: 'Reading Setup', path: '/vi/reading' },
 { name: 'Reading History', path: '/vi/reading/history' },
 { name: 'Reading Session', path: '/vi/reading/session/00000000-0000-0000-0000-000000000000' },
 { name: 'Reader Directory', path: '/vi/readers' },
 { name: 'Reader Detail', path: '/vi/readers/00000000-0000-0000-0000-000000000000' },
 { name: 'Inbox', path: '/vi/chat' },
 { name: 'Chat Room', path: '/vi/chat/00000000-0000-0000-0000-000000000000' },
 { name: 'Collection', path: '/vi/collection' },
 { name: 'Wallet', path: '/vi/wallet' },
 { name: 'Wallet/Deposit', path: '/vi/wallet/deposit' },
 { name: 'Wallet/Withdraw', path: '/vi/wallet/withdraw' },
 { name: 'Profile', path: '/vi/profile' },
 { name: 'Profile/MFA', path: '/vi/profile/mfa' },
 { name: 'Profile/Reader', path: '/vi/profile/reader' },
 { name: 'Reader Apply', path: '/vi/reader/apply' },
 { name: 'Admin Dashboard', path: '/vi/admin' },
 { name: 'Admin/Users', path: '/vi/admin/users' },
 { name: 'Admin/Deposits', path: '/vi/admin/deposits' },
 { name: 'Admin/Promotions', path: '/vi/admin/promotions' },
 { name: 'Admin/Readings', path: '/vi/admin/readings' },
 { name: 'Admin/Reader Requests', path: '/vi/admin/reader-requests' },
 { name: 'Admin/Withdrawals', path: '/vi/admin/withdrawals' },
 { name: 'Admin/Disputes', path: '/vi/admin/disputes' },
];

export const QA_TOKEN_EXP = Math.floor(Date.now() / 1000) + 7 * 24 * 60 * 60;

export const QA_JWT = [
 Buffer.from(JSON.stringify({ alg: 'HS256', typ: 'JWT' })).toString('base64url'),
 Buffer.from(
  JSON.stringify({
   sub: 'qa-user',
   nameid: 'qa-user',
   exp: QA_TOKEN_EXP,
   role: 'admin',
  })
 ).toString('base64url'),
 'qa-signature',
].join('.');

export const QA_AUTH_STATE = JSON.stringify({
 state: {
  user: {
   id: 'qa-user',
   email: 'qa@example.com',
   username: 'qa-user',
   displayName: 'QA User',
   role: 'admin',
  },
 },
 version: 0,
});
