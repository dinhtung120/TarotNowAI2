import { readFileSync } from 'node:fs';
import { globSync } from 'node:fs';
import { resolve } from 'node:path';

const LINE_LIMIT = 120;

const TEMP_ALLOWLIST = {
  'src/features/community/components/PostReportModal.tsx': {
    reason: 'Pending extraction into report form + actions subcomponents',
    expiresAt: '2026-07-01',
  },
  'src/features/reader/presentation/components/readers-directory/ReaderDetailModal.tsx': {
    reason: 'Pending split into summary/actions/sections',
    expiresAt: '2026-07-01',
  },
};

function isAllowlistExpired(expiresAt) {
  const parsed = new Date(expiresAt);
  if (Number.isNaN(parsed.getTime())) {
    return true;
  }

  return parsed.getTime() < Date.now();
}

function collectTargetFiles() {
  const files = globSync('src/**/*.tsx', {
    cwd: process.cwd(),
    nodir: true,
    ignore: [
      '**/*.test.tsx',
      '**/*.spec.tsx',
      '**/__tests__/**',
    ],
  });

  return files.sort((left, right) => left.localeCompare(right));
}

const files = collectTargetFiles();
const violations = [];
const expiredAllowlist = [];

for (const relativePath of files) {
  const absolutePath = resolve(process.cwd(), relativePath);
  const content = readFileSync(absolutePath, 'utf8');
  const lines = content.split('\n').length;
  if (lines <= LINE_LIMIT) {
    continue;
  }

  const allowEntry = TEMP_ALLOWLIST[relativePath];
  if (!allowEntry) {
    violations.push({ relativePath, lines, reason: 'Not in allowlist' });
    continue;
  }

  if (!allowEntry.reason || !allowEntry.expiresAt || isAllowlistExpired(allowEntry.expiresAt)) {
    expiredAllowlist.push({ relativePath, lines, ...allowEntry });
  }
}

if (expiredAllowlist.length > 0) {
  console.error('Component size allowlist has expired entries:');
  for (const entry of expiredAllowlist) {
    console.error(`- ${entry.relativePath}: ${entry.lines} lines (expiresAt=${entry.expiresAt || 'missing'})`);
  }
  process.exit(1);
}

if (violations.length > 0) {
  console.error(`Component size guard failed (limit: ${LINE_LIMIT} lines):`);
  for (const entry of violations) {
    console.error(`- ${entry.relativePath}: ${entry.lines} lines (${entry.reason})`);
  }
  process.exit(1);
}

console.log(`Component size guard passed for ${files.length} files (<= ${LINE_LIMIT} lines, with temporary allowlist).`);
