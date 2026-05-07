import { readFileSync } from 'node:fs';
import { globSync } from 'node:fs';
import { resolve } from 'node:path';
import ts from 'typescript';
import {
 findClientBoundaryViolation,
 findForbiddenFeatureLayerFolder,
 findForbiddenSharedLayerFolder,
 findLine,
 findSensitiveStreamViolation,
 findUnclassifiedRuntimeFiles,
 isAllowedSharedFeatureImport,
 isSharedImportingFeature,
 isTestFile,
 resolveLayer,
} from './lib/cleanArchitectureGuard.mjs';

const sourceFiles = globSync('src/**/*.{ts,tsx,mts}', {
 cwd: process.cwd(),
 nodir: true,
 ignore: ['**/*.d.ts'],
}).filter((path) => !isTestFile(path));

const folderOwnershipViolations = [];
const sharedImportViolations = [];
const clientBoundaryViolations = [];
const sensitiveStreamViolations = [];
const pagePublicApiViolations = [];
const unclassifiedRuntimeFiles = findUnclassifiedRuntimeFiles(sourceFiles);

for (const relativePath of sourceFiles) {
 const sourceCode = readFileSync(resolve(process.cwd(), relativePath), 'utf8');

 const forbiddenSharedLayer = findForbiddenSharedLayerFolder(relativePath);
 if (forbiddenSharedLayer) {
  folderOwnershipViolations.push({
   file: relativePath,
   message: `shared layer folder "${forbiddenSharedLayer}" is forbidden in Frontend; use top-level shared role folders instead.`,
  });
 }

 const forbiddenLayer = findForbiddenFeatureLayerFolder(relativePath);
 if (forbiddenLayer) {
  folderOwnershipViolations.push({
   file: relativePath,
   message: `feature layer folder "${forbiddenLayer}" is forbidden in Frontend; use feature/subfeature role folders instead.`,
  });
 }

 const sourceLayer = resolveLayer(relativePath);
 const clientBoundaryViolation = findClientBoundaryViolation(relativePath, sourceCode);
 if (clientBoundaryViolation) {
  clientBoundaryViolations.push(clientBoundaryViolation);
 }

 const sensitiveStreamViolation = findSensitiveStreamViolation(relativePath, sourceCode);
 if (sensitiveStreamViolation) {
  sensitiveStreamViolations.push(sensitiveStreamViolation);
 }

 if (!sourceLayer) {
  continue;
 }

 const imports = extractImports(sourceCode);
 for (const importPath of imports) {
  if (
   isAppPageOrLayout(relativePath)
   && importPath.startsWith('@/features/')
   && !/^@\/features\/[^/]+\/public$/.test(importPath)
  ) {
   pagePublicApiViolations.push({
    file: relativePath,
    importPath,
    line: findLine(sourceCode, importPath),
   });
  }

  if (isSharedImportingFeature(relativePath, importPath) && !isAllowedSharedFeatureImport(relativePath)) {
   sharedImportViolations.push({
    file: relativePath,
    importPath,
    line: findLine(sourceCode, importPath),
   });
  }
 }
}

if (
 unclassifiedRuntimeFiles.length > 0
 || folderOwnershipViolations.length > 0
 || sharedImportViolations.length > 0
 || clientBoundaryViolations.length > 0
 || sensitiveStreamViolations.length > 0
 || pagePublicApiViolations.length > 0
) {
 console.error('Clean architecture guard failed.');

 if (unclassifiedRuntimeFiles.length > 0) {
  console.error('Unclassified runtime files:');
  for (const filePath of unclassifiedRuntimeFiles) {
   console.error(`- ${filePath}`);
  }
 }

 if (folderOwnershipViolations.length > 0) {
  console.error('Folder ownership violations:');
  for (const violation of folderOwnershipViolations) {
   console.error(`- ${violation.file}: ${violation.message}`);
  }
 }

 if (sharedImportViolations.length > 0) {
  console.error('Shared code must not import feature code:');
  for (const violation of sharedImportViolations) {
   console.error(`- ${violation.file}:${violation.line} import ${violation.importPath}`);
  }
 }

 if (clientBoundaryViolations.length > 0) {
  console.error('Client/runtime boundary violations:');
  for (const violation of clientBoundaryViolations) {
   console.error(`- ${violation.file}:${violation.line} ${violation.message}`);
  }
 }

 if (sensitiveStreamViolations.length > 0) {
  console.error('Sensitive stream payload must not appear on EventSource URLs:');
  for (const violation of sensitiveStreamViolations) {
   console.error(`- ${violation.file}:${violation.line} ${violation.message}`);
  }
 }

 if (pagePublicApiViolations.length > 0) {
  console.error('App page/layout files must import features through feature public APIs only:');
  for (const violation of pagePublicApiViolations) {
   console.error(`- ${violation.file}:${violation.line} import ${violation.importPath}`);
  }
 }

 process.exit(1);
}

console.log(`Clean architecture guard passed (${sourceFiles.length} files checked).`);

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

function isAppPageOrLayout(relativePath) {
 return /^src\/app\/.+\/(page|layout)\.(ts|tsx|mts)$/.test(relativePath);
}

