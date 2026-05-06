import { readFileSync } from 'node:fs';
import { globSync } from 'node:fs';
import { resolve } from 'node:path';

const registryPath = resolve(process.cwd(), 'src/shared/gateways/domainCommandRegistry.ts');
const registrySource = readFileSync(registryPath, 'utf8');

const requiredCommandKeys = [
 'wallet.withdrawal.admin.process',
 'admin.user.adjust-balance',
 'chat.dispute.resolve',
 'reading.session.init',
 'wallet.deposit.admin.process',
 'admin.reader-request.process',
];

const commandKeyViolations = requiredCommandKeys.filter((key) => {
 const pattern = new RegExp(`['"]${escapeRegex(key)}['"]\\s*:`);
 return !pattern.test(registrySource);
});

const expectedCommandUsage = [
 {
  file: 'src/features/wallet/withdraw/actions/admin.ts',
  key: 'wallet.withdrawal.admin.process',
 },
 {
  file: 'src/features/chat/disputes/conversations.finance.ts',
  key: 'chat.dispute.resolve',
 },
 {
  file: 'src/features/admin/users/actions/users.ts',
  key: 'admin.user.adjust-balance',
 },
 {
  file: 'src/app/api/reading/init/route.ts',
  key: 'reading.session.init',
 },
 {
  file: 'src/features/admin/deposits/actions/deposits.ts',
  key: 'wallet.deposit.admin.process',
 },
 {
  file: 'src/features/admin/reader-requests/actions/reader-requests.ts',
  key: 'admin.reader-request.process',
 },
];

const usageViolations = expectedCommandUsage
 .map((entry) => {
  const source = readFileSync(resolve(process.cwd(), entry.file), 'utf8');
  return source.includes(`'${entry.key}'`) || source.includes(`"${entry.key}"`)
   ? null
   : entry;
 })
 .filter(Boolean);

const updateUserFlowSource = readFileSync(
 resolve(process.cwd(), 'src/features/admin/users/hooks/useAdminUsers.ts'),
 'utf8',
);
const updateUserFlowViolation = updateUserFlowSource.includes('enforceMoneyEvent')
 || updateUserFlowSource.includes('hasBalanceChanged');

const sensitiveDirectCallDetectors = [
 {
  label: '/admin/withdrawals/process',
  pattern: /serverHttpRequest(?:<[^>]+>)?\s*\(\s*['"`]\/admin\/withdrawals\/process['"`]/,
 },
 {
  label: '/admin/disputes/{id}/resolve',
  pattern: /serverHttpRequest(?:<[^>]+>)?\s*\(\s*`\/admin\/disputes\/\$\{[^}]+\}\/resolve`/,
 },
 {
  label: '/reading/init',
  pattern: /serverHttpRequest(?:<[^>]+>)?\s*\(\s*['"`]\/reading\/init['"`]/,
 },
 {
  label: '/admin/users/{id}',
  pattern: /serverHttpRequest(?:<[^>]+>)?\s*\(\s*`\/admin\/users\/\$\{[^}]+\}`/,
 },
 {
  label: '/admin/deposits/process',
  pattern: /serverHttpRequest(?:<[^>]+>)?\s*\(\s*['"`]\/admin\/deposits\/process['"`]/,
 },
 {
  label: '/admin/reader-requests/process',
  pattern: /serverHttpRequest(?:<[^>]+>)?\s*\(\s*['"`]\/admin\/reader-requests\/process['"`]/,
 },
];

const sourceFiles = globSync('src/**/*.{ts,tsx}', {
 cwd: process.cwd(),
 nodir: true,
 ignore: [],
}).filter((path) => !isTestFile(path));

const directCallViolations = [];
for (const relativePath of sourceFiles) {
 if (relativePath === 'src/shared/gateways/domainCommandRegistry.ts') {
  continue;
 }

 const source = readFileSync(resolve(process.cwd(), relativePath), 'utf8');
 if (
  relativePath.startsWith('src/features/gamification/')
  && (source.includes('getPublicApiBaseUrl(') || source.includes('NEXT_PUBLIC_API_URL'))
 ) {
  directCallViolations.push({
   file: relativePath,
   pathPattern: 'admin gamification client must not read public backend origin directly',
  });
  continue;
 }

 for (const detector of sensitiveDirectCallDetectors) {
  if (!detector.pattern.test(source)) {
   continue;
  }

  directCallViolations.push({
   file: relativePath,
   pathPattern: detector.label,
  });
  break;
 }
}

if (commandKeyViolations.length > 0 || usageViolations.length > 0 || updateUserFlowViolation || directCallViolations.length > 0) {
 console.error('Domain event evidence verification failed.');

 if (commandKeyViolations.length > 0) {
  console.error('Missing command keys in DomainCommandRegistry:');
  for (const key of commandKeyViolations) {
   console.error(`- ${key}`);
  }
 }

 if (usageViolations.length > 0) {
  console.error('Missing invokeDomainCommand usage for required files:');
  for (const violation of usageViolations) {
   console.error(`- ${violation.file} (expected key: ${violation.key})`);
  }
 }

 if (updateUserFlowViolation) {
  console.error('Admin users flow still contains UI-side money-event decision logic.');
 }

 if (directCallViolations.length > 0) {
  console.error('Sensitive paths still use direct serverHttpRequest:');
  for (const violation of directCallViolations) {
   console.error(`- ${violation.file} (matched: ${violation.pathPattern})`);
  }
 }

 process.exit(1);
}

console.log(`Domain event evidence verification passed (${requiredCommandKeys.length} required command keys validated).`);

function escapeRegex(value) {
 return value.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
}

function isTestFile(path) {
 return path.includes('.test.')
  || path.includes('.spec.')
  || path.includes('/__tests__/');
}
