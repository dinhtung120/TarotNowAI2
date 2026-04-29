import { existsSync, readdirSync, readFileSync, writeFileSync } from 'node:fs';
import { resolve } from 'node:path';

const BASELINE_PATH = resolve(process.cwd(), 'scripts/hook-action-size-baseline.json');
const ROOT_PATH = resolve(process.cwd(), 'src');
const HARD_LIMIT = Number(process.env.HOOK_ACTION_SIZE_LIMIT ?? '320');
const SYNC_FLAG = process.argv.includes('--sync') || process.env.HOOK_ACTION_SIZE_SYNC === '1';

function normalizePath(value) {
 return value.replace(/\\/g, '/');
}

function isTrackedFile(relativePath) {
 if (!relativePath.endsWith('.ts') && !relativePath.endsWith('.tsx')) {
  return false;
 }

 if (/(\.test\.|\.spec\.)tsx?$/.test(relativePath)) {
  return false;
 }

 if (relativePath.includes('/__tests__/')) {
  return false;
 }

 return relativePath.startsWith('src/shared/application/')
  || /^src\/features\/[^/]+\/application\/.+/.test(relativePath);
}

function listTrackedFiles(directoryPath, prefix = '') {
 const entries = readdirSync(directoryPath, { withFileTypes: true });
 const files = [];

 for (const entry of entries) {
  const relativePath = normalizePath(`${prefix}${entry.name}`);
  const absolutePath = resolve(directoryPath, entry.name);
  if (entry.isDirectory()) {
   files.push(...listTrackedFiles(absolutePath, `${relativePath}/`));
   continue;
  }

  if (isTrackedFile(relativePath)) {
   files.push(relativePath);
  }
 }

 return files;
}

function getLineCount(relativePath) {
 const absolutePath = resolve(process.cwd(), relativePath);
 const source = readFileSync(absolutePath, 'utf8').replace(/\r\n/g, '\n');
 return source.endsWith('\n')
  ? source.slice(0, -1).split('\n').length
  : source.split('\n').length;
}

const trackedFiles = listTrackedFiles(ROOT_PATH, 'src/').sort((a, b) => a.localeCompare(b));
if (trackedFiles.length === 0) {
 console.error('Hook/action size guard found no tracked files in scope.');
 process.exit(1);
}

if (SYNC_FLAG) {
 const entries = Object.fromEntries(
  trackedFiles.map((relativePath) => [relativePath, getLineCount(relativePath)]),
 );

 writeFileSync(BASELINE_PATH, `${JSON.stringify({ entries }, null, 2)}\n`, 'utf8');
 console.log(`Hook/action size baseline synced (${trackedFiles.length} files).`);
 process.exit(0);
}

if (!existsSync(BASELINE_PATH)) {
 console.error('Hook/action size baseline is missing.');
 console.error('Run `node scripts/check-hook-action-size.mjs --sync` to generate baseline.');
 process.exit(1);
}

const baseline = JSON.parse(readFileSync(BASELINE_PATH, 'utf8'));
const entries = baseline.entries ?? {};
const violations = [];

for (const relativePath of trackedFiles) {
 const baselineLines = entries[relativePath];
 const lineCount = getLineCount(relativePath);

 if (lineCount > HARD_LIMIT) {
  violations.push(`${relativePath} exceeds hard limit (${lineCount} > ${HARD_LIMIT}).`);
  continue;
 }

 if (baselineLines === undefined) {
  violations.push(`${relativePath} is new in guard scope but missing baseline entry.`);
  continue;
 }

 if (lineCount > Number(baselineLines)) {
  violations.push(`${relativePath} grew (${lineCount} > baseline ${baselineLines}).`);
 }
}

for (const relativePath of Object.keys(entries)) {
 if (!trackedFiles.includes(relativePath)) {
  violations.push(`${relativePath} is in baseline but outside current tracked scope.`);
 }
}

if (violations.length > 0) {
 console.error('Hook/action size guard failed.');
 for (const violation of violations) {
  console.error(`- ${violation}`);
 }
 console.error('Run `node scripts/check-hook-action-size.mjs --sync` after approved structural refactors.');
 process.exit(1);
}

console.log(`Hook/action size guard passed (${trackedFiles.length} tracked files).`);
