import { existsSync, readFileSync } from 'node:fs';
import { resolve } from 'node:path';

const BASELINE_PATH = resolve(process.cwd(), 'scripts/hook-action-size-baseline.json');
const HARD_LIMIT = Number(process.env.HOOK_ACTION_SIZE_LIMIT ?? '320');

if (!existsSync(BASELINE_PATH)) {
 console.error('Hook/action size baseline is missing.');
 process.exit(1);
}

const baseline = JSON.parse(readFileSync(BASELINE_PATH, 'utf8'));
const entries = baseline.entries ?? {};
const violations = [];

for (const [relativePath, baselineLines] of Object.entries(entries)) {
 const absolutePath = resolve(process.cwd(), relativePath);
 if (!existsSync(absolutePath)) {
  violations.push(`${relativePath} is missing but still present in baseline.`);
  continue;
 }

 const source = readFileSync(absolutePath, 'utf8').replace(/\r\n/g, '\n');
 const lineCount = source.endsWith('\n')
  ? source.slice(0, -1).split('\n').length
  : source.split('\n').length;

 if (lineCount > HARD_LIMIT) {
  violations.push(`${relativePath} exceeds hard limit (${lineCount} > ${HARD_LIMIT}).`);
  continue;
 }

 if (lineCount > Number(baselineLines)) {
  violations.push(`${relativePath} grew (${lineCount} > baseline ${baselineLines}).`);
  continue;
 }
}

if (violations.length > 0) {
 console.error('Hook/action size guard failed.');
 for (const violation of violations) {
  console.error(`- ${violation}`);
 }
 process.exit(1);
}

console.log(`Hook/action size guard passed (${Object.keys(entries).length} tracked files).`);
