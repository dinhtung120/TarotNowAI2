import { existsSync, readFileSync } from 'node:fs';
import { globSync } from 'node:fs';
import { dirname, extname, resolve } from 'node:path';
import ts from 'typescript';

const layerOrder = {
 domain: 0,
 application: 1,
 infrastructure: 2,
 presentation: 3,
};

const sourceFiles = globSync('src/**/*.{ts,tsx,mts}', {
 cwd: process.cwd(),
 nodir: true,
 ignore: ['**/*.d.ts'],
}).filter((path) => !isTestFile(path));

const dependencyViolations = [];
const domainPurityViolations = [];

for (const relativePath of sourceFiles) {
 const sourceLayer = resolveLayer(relativePath);
 if (!sourceLayer) {
  continue;
 }

 const sourceCode = readFileSync(resolve(process.cwd(), relativePath), 'utf8');
 const imports = extractImports(sourceCode);
 for (const importPath of imports) {
  if (sourceLayer === 'domain' && isExternalImport(importPath)) {
   domainPurityViolations.push({
    file: relativePath,
    importPath,
    line: findLine(sourceCode, importPath),
   });
   continue;
  }

  const targetRelativePath = resolveImportTarget(relativePath, importPath);
  if (!targetRelativePath) {
   continue;
  }

  const targetLayer = resolveLayer(targetRelativePath);
  if (!targetLayer) {
   continue;
  }

  if (isAllowedLayerException(relativePath, sourceLayer, targetLayer)) {
   continue;
  }

  const sourceRank = layerOrder[sourceLayer];
  const targetRank = layerOrder[targetLayer];
  if (sourceRank < targetRank) {
   dependencyViolations.push({
    file: relativePath,
    sourceLayer,
    targetLayer,
    importPath,
    line: findLine(sourceCode, importPath),
   });
  }
 }
}

if (dependencyViolations.length > 0 || domainPurityViolations.length > 0) {
 console.error('Clean architecture guard failed.');

 if (dependencyViolations.length > 0) {
  console.error('Layer direction violations:');
  for (const violation of dependencyViolations) {
   console.error(
    `- ${violation.file}:${violation.line} (${violation.sourceLayer} -> ${violation.targetLayer}) import ${violation.importPath}`,
   );
  }
 }

 if (domainPurityViolations.length > 0) {
  console.error('Domain purity violations (external imports are forbidden in domain layer):');
  for (const violation of domainPurityViolations) {
   console.error(`- ${violation.file}:${violation.line} import ${violation.importPath}`);
  }
 }

 process.exit(1);
}

console.log(`Clean architecture guard passed (${sourceFiles.length} files checked).`);

function resolveLayer(path) {
 if (path.includes('/domain/')) return 'domain';
 if (path.includes('/application/')) return 'application';
 if (path.includes('/infrastructure/')) return 'infrastructure';
 if (path.includes('/presentation/')) return 'presentation';
 return null;
}

function extractImports(sourceCode) {
 const imports = new Set();
 const sourceFile = ts.createSourceFile(
  'architecture-check.ts',
  sourceCode,
  ts.ScriptTarget.Latest,
  true,
  ts.ScriptKind.TSX,
 );

 const visit = (node) => {
  if (ts.isImportDeclaration(node) || ts.isExportDeclaration(node)) {
   const moduleSpecifier = node.moduleSpecifier;
   if (moduleSpecifier && ts.isStringLiteralLike(moduleSpecifier)) {
    imports.add(moduleSpecifier.text);
   }
  }

  if (ts.isCallExpression(node) && node.expression.kind === ts.SyntaxKind.ImportKeyword) {
   const [argument] = node.arguments;
   if (argument && ts.isStringLiteralLike(argument)) {
    imports.add(argument.text);
   }
  }

  ts.forEachChild(node, visit);
 };

 visit(sourceFile);

 return imports;
}

function isExternalImport(importPath) {
 return !importPath.startsWith('./')
  && !importPath.startsWith('../')
  && !importPath.startsWith('@/');
}

function resolveImportTarget(sourceRelativePath, importPath) {
 if (importPath.startsWith('@/')) {
  return `src/${importPath.slice(2)}`;
 }

 if (!importPath.startsWith('.')) {
  return null;
 }

 const sourceDir = dirname(sourceRelativePath);
 const withExtension = resolveWithExtension(resolve(process.cwd(), sourceDir, importPath));
 if (!withExtension) {
  return null;
 }

 return toPosixRelative(withExtension);
}

function resolveWithExtension(absolutePath) {
 const extensions = ['.ts', '.tsx', '.mts'];
 if (extensions.includes(extname(absolutePath))) {
  return absolutePath;
 }

 for (const extension of extensions) {
  const candidate = `${absolutePath}${extension}`;
  if (existsSync(candidate)) {
   return candidate;
  }
 }

 for (const extension of extensions) {
  const indexCandidate = resolve(absolutePath, `index${extension}`);
  if (existsSync(indexCandidate)) {
   return indexCandidate;
  }
 }

 return null;
}

function toPosixRelative(absolutePath) {
 return absolutePath
  .replace(resolve(process.cwd()), '')
  .replace(/^\/+/, '')
  .replace(/\\/g, '/');
}

function isAllowedLayerException(filePath, sourceLayer, targetLayer) {
 if (sourceLayer === 'application'
  && targetLayer === 'infrastructure'
  && filePath.includes('/application/gateways/')) {
  return true;
 }

 return false;
}

function findLine(sourceCode, snippet) {
 const index = sourceCode.indexOf(snippet);
 if (index < 0) {
  return 1;
 }

 return sourceCode.slice(0, index).split('\n').length;
}

function isTestFile(path) {
 return path.includes('.test.')
  || path.includes('.spec.')
  || path.includes('/__tests__/');
}
