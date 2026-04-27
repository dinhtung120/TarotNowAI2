import { readFileSync } from 'node:fs';
import { globSync } from 'node:fs';
import { resolve } from 'node:path';

const applicationFiles = globSync('src/**/application/**/*.{ts,tsx}', {
 cwd: process.cwd(),
 nodir: true,
 ignore: ['**/*.d.ts'],
}).filter((path) => !path.includes('/shared/application/gateways/'));

const sharedInfrastructureImportRegex = /from\s+['\"]@\/shared\/infrastructure\//g;
const violatingFiles = [];

for (const relativePath of applicationFiles) {
 const absolutePath = resolve(process.cwd(), relativePath);
 const source = readFileSync(absolutePath, 'utf8');
 if (!sharedInfrastructureImportRegex.test(source)) {
  continue;
 }

 violatingFiles.push(relativePath);
}

violatingFiles.sort((left, right) => left.localeCompare(right));

if (violatingFiles.length > 0) {
 console.error('Clean architecture guard failed: application layer must not import shared/infrastructure directly.');
 for (const filePath of violatingFiles) {
  console.error(`- ${filePath}`);
 }
 process.exit(1);
}

console.log(`Clean architecture guard passed (${applicationFiles.length} application files, 0 shared/infrastructure direct imports).`);
