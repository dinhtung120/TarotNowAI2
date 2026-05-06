import { relative, resolve, sep } from 'node:path';
import { readdirSync, readFileSync } from 'node:fs';
import { fileURLToPath } from 'node:url';

const root = resolve(fileURLToPath(new URL('..', import.meta.url)));
const srcRoot = resolve(root, 'src');
const sharedRoot = resolve(srcRoot, 'shared');
const extensions = ['.ts', '.tsx'];
const importPattern = /(?:import|export)\s+(?:type\s+)?(?:[^'\"]*?from\s+)?['\"]([^'\"]+)['\"]|import\s*\(\s*['\"]([^'\"]+)['\"]\s*\)/g;

function walk(dir) {
  const entries = readdirSync(dir, { withFileTypes: true });
  const files = [];

  for (const entry of entries) {
    const path = resolve(dir, entry.name);

    if (entry.isDirectory()) {
      if (entry.name === 'node_modules' || entry.name === '.next') continue;
      files.push(...walk(path));
      continue;
    }

    if (entry.isFile() && extensions.some((extension) => path.endsWith(extension))) {
      files.push(path);
    }
  }

  return files;
}

function stripExtension(path) {
  for (const extension of extensions) {
    if (path.endsWith(extension)) return path.slice(0, -extension.length);
  }

  return path;
}

function sharedSpecifiersFor(file) {
  const rel = relative(sharedRoot, file).split(sep).join('/');
  const withoutExtension = stripExtension(rel);

  const specifiers = new Set([
    `@/shared/${withoutExtension}`,
    `@/shared/${withoutExtension}/index`,
  ]);

  if (withoutExtension.endsWith('/index')) {
    specifiers.add(`@/shared/${withoutExtension.slice(0, -'/index'.length)}`);
  }

  return specifiers;
}

function areaFor(importer) {
  const rel = relative(srcRoot, importer).split(sep).join('/');
  const parts = rel.split('/');

  if (parts[0] === 'shared') return 'shared';
  if (parts[0] === 'features') return `features/${parts[1]}`;
  if (parts[0] === 'app') return 'app';

  return parts[0];
}

function importsBySpecifier(files) {
  const imports = new Map();

  for (const file of files) {
    const source = readFileSync(file, 'utf8');
    let match;

    while ((match = importPattern.exec(source))) {
      const specifier = match[1] ?? match[2];
      if (!specifier?.startsWith('@/shared/')) continue;

      if (!imports.has(specifier)) imports.set(specifier, []);
      imports.get(specifier).push(file);
    }
  }

  return imports;
}

const sourceFiles = walk(srcRoot);
const sharedFiles = walk(sharedRoot)
  .filter((file) => !file.endsWith('.test.ts') && !file.endsWith('.test.tsx'))
  .sort();
const imports = importsBySpecifier(sourceFiles);

console.log('| File | Outside areas | Outside importers | Shared importers | Decision |');
console.log('| --- | --- | --- | --- | --- |');

for (const file of sharedFiles) {
  const specifiers = sharedSpecifiersFor(file);
  const importers = [...specifiers].flatMap((specifier) => imports.get(specifier) ?? []);
  const sharedImporters = importers.filter((importer) => !relative(sharedRoot, importer).startsWith('..'));
  const outsideImporters = importers.filter((importer) => relative(sharedRoot, importer).startsWith('..'));
  const areas = [...new Set(outsideImporters.map(areaFor))].sort();
  const decision = areas.length >= 2 ? 'keep' : areas.length === 1 ? `move:${areas[0]}` : 'review-unused';

  const relFile = relative(root, file).split(sep).join('/');
  const outsideList = outsideImporters.map((importer) => relative(root, importer).split(sep).join('/')).sort().join('<br>');
  const sharedList = sharedImporters.map((importer) => relative(root, importer).split(sep).join('/')).sort().join('<br>');

  console.log(`| ${relFile} | ${areas.join('<br>')} | ${outsideList} | ${sharedList} | ${decision} |`);
}
