import { existsSync, readFileSync } from 'node:fs';
import { globSync } from 'node:fs';
import { resolve } from 'node:path';

const HARD_LIMIT = Number(process.env.COMPONENT_SIZE_LIMIT ?? '120');
const RATCHET_LIMIT = Number(process.env.COMPONENT_SIZE_RATCHET_LIMIT ?? '100');
const BASELINE_PATH = resolve(process.cwd(), 'scripts/component-size-baseline.json');

function collectTargetFiles() {
 const files = globSync('src/**/*.tsx', {
  cwd: process.cwd(),
  nodir: true,
 });

 return files
  .filter((path) => !isIgnoredComponentFile(path))
  .sort((left, right) => left.localeCompare(right));
}

function isIgnoredComponentFile(path) {
 return path.includes('.test.')
  || path.includes('.spec.')
  || path.includes('/__tests__/');
}

function countLines(content) {
 const normalized = content.replace(/\r\n/g, '\n');
 const withoutTrailingNewline = normalized.endsWith('\n')
  ? normalized.slice(0, -1)
  : normalized;

 if (withoutTrailingNewline.length === 0) {
  return 0;
 }

 return withoutTrailingNewline.split('\n').length;
}

function loadBaseline() {
 if (!existsSync(BASELINE_PATH)) {
  console.error('Component size baseline file is missing:', BASELINE_PATH);
  process.exit(1);
 }

 const baselineRaw = readFileSync(BASELINE_PATH, 'utf8');
 const baseline = JSON.parse(baselineRaw);
 const ratchetLimitFromBaseline = Number(baseline.ratchetLimit);
 if (!Number.isFinite(ratchetLimitFromBaseline) || ratchetLimitFromBaseline !== RATCHET_LIMIT) {
  console.error(
   `Component size baseline ratchet mismatch: expected ${RATCHET_LIMIT}, got ${baseline.ratchetLimit}`,
  );
  process.exit(1);
 }

 if (!baseline.entries || typeof baseline.entries !== 'object') {
  console.error('Invalid component-size baseline format: missing entries map.');
  process.exit(1);
 }

 return baseline.entries;
}

const baselineEntries = loadBaseline();
const files = collectTargetFiles();
const currentLineByFile = new Map();

const hardLimitViolations = [];
const newRatchetViolations = [];
const ratchetGrowthViolations = [];
const baselineStaleEntries = [];
const baselineNeedsTightening = [];

for (const relativePath of files) {
 const absolutePath = resolve(process.cwd(), relativePath);
 const content = readFileSync(absolutePath, 'utf8');
 const lines = countLines(content);
 currentLineByFile.set(relativePath, lines);

 if (lines > HARD_LIMIT) {
  hardLimitViolations.push({ relativePath, lines });
 }

 if (lines <= RATCHET_LIMIT) {
  continue;
 }

 const baselineLines = baselineEntries[relativePath];
 if (typeof baselineLines !== 'number') {
  newRatchetViolations.push({ relativePath, lines });
  continue;
 }

 if (lines > baselineLines) {
  ratchetGrowthViolations.push({ relativePath, lines, baselineLines });
  continue;
 }

 if (lines < baselineLines) {
  baselineNeedsTightening.push({ relativePath, lines, baselineLines });
 }
}

for (const [relativePath, baselineLines] of Object.entries(baselineEntries)) {
 const currentLines = currentLineByFile.get(relativePath);
 if (typeof currentLines !== 'number') {
  baselineStaleEntries.push({ relativePath, baselineLines, reason: 'missing file' });
  continue;
 }

 if (currentLines <= RATCHET_LIMIT) {
  baselineStaleEntries.push({
   relativePath,
   baselineLines,
   reason: `no longer exceeds ratchet (${currentLines} <= ${RATCHET_LIMIT})`,
  });
 }
}

if (
 hardLimitViolations.length > 0
 || newRatchetViolations.length > 0
 || ratchetGrowthViolations.length > 0
 || baselineStaleEntries.length > 0
 || baselineNeedsTightening.length > 0
) {
 console.error('Component size guard failed.');

 if (hardLimitViolations.length > 0) {
  console.error(`Hard limit violations (> ${HARD_LIMIT} lines):`);
  for (const violation of hardLimitViolations) {
   console.error(`- ${violation.relativePath}: ${violation.lines} lines`);
  }
 }

 if (newRatchetViolations.length > 0) {
  console.error(`New ratchet debt violations (> ${RATCHET_LIMIT} lines and not in baseline):`);
  for (const violation of newRatchetViolations) {
   console.error(`- ${violation.relativePath}: ${violation.lines} lines`);
  }
 }

 if (ratchetGrowthViolations.length > 0) {
  console.error('Ratchet growth violations (existing debt got larger):');
  for (const violation of ratchetGrowthViolations) {
   console.error(
    `- ${violation.relativePath}: ${violation.lines} lines (baseline ${violation.baselineLines})`,
   );
  }
 }

 if (baselineStaleEntries.length > 0) {
  console.error('Baseline stale entries must be removed:');
  for (const violation of baselineStaleEntries) {
   console.error(`- ${violation.relativePath}: baseline ${violation.baselineLines} (${violation.reason})`);
  }
 }

 if (baselineNeedsTightening.length > 0) {
  console.error('Baseline must be tightened where file size decreased:');
  for (const violation of baselineNeedsTightening) {
   console.error(
    `- ${violation.relativePath}: current ${violation.lines}, baseline ${violation.baselineLines}`,
   );
  }
 }

 process.exit(1);
}

const debtCount = Array.from(currentLineByFile.values()).filter((lines) => lines > RATCHET_LIMIT).length;
console.log(
 `Component size guard passed for ${files.length} files (hard limit: ${HARD_LIMIT}, ratchet: ${RATCHET_LIMIT}, baseline debt: ${debtCount}).`,
);
