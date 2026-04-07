import { promises as fs } from 'node:fs';
import path from 'node:path';

const ROOT = path.resolve(process.cwd(), 'src');

async function collectFiles(dir) {
  const entries = await fs.readdir(dir, { withFileTypes: true });
  const files = await Promise.all(entries.map(async (entry) => {
    const fullPath = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      return collectFiles(fullPath);
    }

    if (entry.isFile() && (fullPath.endsWith('.ts') || fullPath.endsWith('.tsx'))) {
      return [fullPath];
    }

    return [];
  }));

  return files.flat();
}

function shouldKeepComment(trimmed) {
  return trimmed.startsWith('// eslint')
    || trimmed.startsWith('// @ts-')
    || trimmed.startsWith('// #')
    || trimmed.startsWith('///');
}

function stripStandaloneLineComments(content) {
  const lines = content.split('\n');
  const nextLines = lines.filter((line) => {
    const trimmed = line.trimStart();
    if (!trimmed.startsWith('//')) {
      return true;
    }

    return shouldKeepComment(trimmed);
  });

  return nextLines.join('\n').replace(/\n{3,}/g, '\n\n');
}

async function run() {
  const files = await collectFiles(ROOT);
  let changed = 0;

  for (const file of files) {
    const original = await fs.readFile(file, 'utf8');
    const next = stripStandaloneLineComments(original);

    if (next !== original) {
      await fs.writeFile(file, next, 'utf8');
      changed++;
    }
  }

  console.log(`Stripped standalone line comments in ${changed} files.`);
}

run().catch((error) => {
  console.error(error);
  process.exit(1);
});
