import { existsSync, readFileSync } from 'node:fs';
import { resolve } from 'node:path';

const COVERAGE_SUMMARY_PATH = resolve(process.cwd(), 'coverage/coverage-summary.json');

const RISK_COVERAGE_RULES = [
 {
  file: 'src/shared/server/auth/protectedRouteAuthDecision.ts',
  thresholds: {
   statements: 90,
   branches: 75,
   functions: 90,
   lines: 90,
  },
 },
 {
  file: 'src/shared/infrastructure/auth/serverAuth.ts',
  thresholds: {
   statements: 70,
   branches: 55,
   functions: 75,
   lines: 72,
  },
 },
 {
  file: 'src/shared/application/gateways/domainCommandRegistry.ts',
  thresholds: {
   statements: 65,
   branches: 55,
   functions: 70,
   lines: 65,
  },
 },
 {
  file: 'src/features/admin/application/actions/users.ts',
  thresholds: {
   statements: 45,
   branches: 30,
   functions: 45,
   lines: 45,
  },
 },
 {
  file: 'src/features/wallet/application/actions/deposit/user-orders.ts',
  thresholds: {
   statements: 75,
   branches: 60,
   functions: 75,
   lines: 72,
  },
 },
 {
  file: 'src/store/authStore.ts',
  thresholds: {
   statements: 75,
   branches: 60,
   functions: 80,
   lines: 75,
  },
 },
 {
  file: 'src/shared/server/auth/redirectAuthenticatedAuthEntry.ts',
  thresholds: {
   statements: 85,
   branches: 75,
   functions: 85,
   lines: 85,
  },
 },
 {
  file: 'src/features/notifications/application/notificationCache.ts',
  thresholds: {
   statements: 90,
   branches: 80,
   functions: 90,
   lines: 90,
  },
 },
 {
  file: 'src/shared/application/hooks/useHydrateFormOnce.ts',
  thresholds: {
   statements: 90,
   branches: 80,
   functions: 90,
   lines: 90,
  },
 },
 {
  file: 'src/shared/application/hooks/useReconnectWakeup.ts',
  thresholds: {
   statements: 90,
   branches: 80,
   functions: 90,
   lines: 90,
  },
 },
 {
  file: 'src/app/[locale]/api/reading/sessions/[sessionId]/streamRouteGuards.ts',
  thresholds: {
   statements: 90,
   branches: 80,
   functions: 90,
   lines: 90,
  },
 },
 {
  file: 'src/app/[locale]/api/reading/sessions/[sessionId]/stream/route.ts',
  thresholds: {
   statements: 85,
   branches: 70,
   functions: 85,
   lines: 85,
  },
 },
 {
  file: 'src/app/api/_shared/problemDetails.ts',
  thresholds: {
   statements: 90,
   branches: 70,
   functions: 90,
   lines: 90,
  },
 },
 {
  file: 'src/features/gamification/admin/application/adminGamificationFormSchema.ts',
  thresholds: {
   statements: 75,
   branches: 60,
   functions: 75,
   lines: 75,
  },
 },
 {
  file: 'src/features/chat/application/chat-connection/useChatSignalRLifecycle.ts',
  thresholds: {
   statements: 55,
   branches: 40,
   functions: 60,
   lines: 55,
  },
 },
 {
  file: 'src/shared/application/hooks/usePresenceConnection.ts',
  thresholds: {
   statements: 70,
   branches: 50,
   functions: 80,
   lines: 70,
  },
 },
 {
  file: 'src/features/notifications/application/useNotificationDropdown.ts',
  thresholds: {
   statements: 75,
   branches: 60,
   functions: 80,
   lines: 75,
  },
 },
 {
  file: 'src/features/notifications/application/useNotificationsPage.ts',
  thresholds: {
   statements: 80,
   branches: 60,
   functions: 90,
   lines: 80,
  },
 },
 {
  file: 'src/features/profile/application/useProfilePage.ts',
  thresholds: {
   statements: 55,
   branches: 40,
   functions: 55,
   lines: 55,
  },
 },
 {
  file: 'src/features/admin/system-configs/application/useAdminSystemConfigs.ts',
  thresholds: {
   statements: 55,
   branches: 40,
   functions: 55,
   lines: 55,
  },
 },
 {
  file: 'src/features/admin/promotions/application/useAdminPromotions.ts',
  thresholds: {
   statements: 55,
   branches: 40,
   functions: 55,
   lines: 55,
  },
 },
 {
  file: 'src/features/gamification/admin/application/useAdminGamification.ts',
  thresholds: {
   statements: 75,
   branches: 50,
   functions: 80,
   lines: 75,
  },
 },
];

function normalizePath(filePath) {
 return filePath.replace(/\\/g, '/');
}

function toRepoRelative(filePath) {
 const normalized = normalizePath(filePath);
 const frontendMarker = '/Frontend/';
 const markerIndex = normalized.lastIndexOf(frontendMarker);
 if (markerIndex >= 0) {
  return normalized.slice(markerIndex + frontendMarker.length);
 }

 if (normalized.startsWith('src/')) {
  return normalized;
 }

 const srcIndex = normalized.lastIndexOf('/src/');
 if (srcIndex >= 0) {
  return normalized.slice(srcIndex + 1);
 }

 return normalized;
}

if (!existsSync(COVERAGE_SUMMARY_PATH)) {
 console.error(`Coverage summary not found: ${COVERAGE_SUMMARY_PATH}`);
 console.error('Run `vitest run --coverage` before executing risk coverage gate.');
 process.exit(1);
}

const summary = JSON.parse(readFileSync(COVERAGE_SUMMARY_PATH, 'utf8'));
const indexedCoverage = new Map();
for (const [filePath, coverage] of Object.entries(summary)) {
 if (filePath === 'total') {
  continue;
 }

 indexedCoverage.set(toRepoRelative(filePath), coverage);
}

const violations = [];
for (const rule of RISK_COVERAGE_RULES) {
 const coverage = indexedCoverage.get(rule.file);
 if (!coverage) {
  violations.push({
   type: 'missing-file-coverage',
   file: rule.file,
   detail: 'No coverage entry found for risk-tier file.',
  });
  continue;
 }

 for (const [metric, threshold] of Object.entries(rule.thresholds)) {
  const current = Number(coverage[metric]?.pct ?? 0);
  if (current < threshold) {
   violations.push({
    type: 'threshold',
    file: rule.file,
    metric,
    threshold,
    current,
   });
  }
 }
}

if (violations.length > 0) {
 console.error('Risk-tier coverage gate failed.');
 for (const violation of violations) {
  if (violation.type === 'missing-file-coverage') {
   console.error(`- ${violation.file}: ${violation.detail}`);
   continue;
  }

  console.error(
   `- ${violation.file}: ${violation.metric} ${violation.current.toFixed(2)}% < ${violation.threshold}%`,
  );
 }

 process.exit(1);
}

console.log(`Risk-tier coverage gate passed (${RISK_COVERAGE_RULES.length} files checked).`);
