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

function stripLongBlockComments(content) {
  let next = content.replace(/\/\*[\s\S]*?\*\//g, (match) => {
    const lines = match.split('\n').length;
    return lines >= 4 ? '' : match;
  });

  next = next.replace(/\n{3,}/g, '\n\n');
  return next;
}

async function run() {
  const files = await collectFiles(ROOT);
  let changed = 0;

  for (const file of files) {
    const original = await fs.readFile(file, 'utf8');
    const next = stripLongBlockComments(original);

    if (next !== original) {
      await fs.writeFile(file, next, 'utf8');
      changed++;
    }
  }

  console.log(`Stripped long block comments in ${changed} files.`);
}

run().catch((error) => {
  console.error(error);
  process.exit(1);
});
