import { readFileSync } from 'node:fs';
import { globSync } from 'node:fs';
import { resolve } from 'node:path';

const ALLOWLIST_PATH = resolve(process.cwd(), 'scripts/image-unoptimized-allowlist.json');
const files = globSync('src/**/*.tsx', {
 cwd: process.cwd(),
 nodir: true,
 ignore: ['**/*.test.*', '**/*.spec.*', '**/__tests__/**'],
});
const allowlist = new Set(JSON.parse(readFileSync(ALLOWLIST_PATH, 'utf8')).files ?? []);

const violations = [];
for (const relativePath of files) {
 const source = readFileSync(resolve(process.cwd(), relativePath), 'utf8');
 if (!source.includes('unoptimized')) {
  continue;
 }

 if (source.includes('image-policy:allow-unoptimized')) {
  continue;
 }

 if (allowlist.has(relativePath)) {
  continue;
 }

 violations.push(relativePath);
}

if (violations.length > 0) {
 console.error('Next image policy guard failed. `unoptimized` requires explicit justification comment.');
 for (const filePath of violations) {
  console.error(`- ${filePath}`);
 }
 process.exit(1);
}

console.log('Next image policy guard passed.');
