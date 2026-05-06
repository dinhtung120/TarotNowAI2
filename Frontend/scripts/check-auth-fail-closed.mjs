import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';

const protectedDecisionPath = resolve(
 process.cwd(),
 'src/shared/server/auth/protectedRouteAuthDecision.ts',
);
const serverAuthPath = resolve(
 process.cwd(),
 'src/shared/auth/serverAuth.ts',
);

const protectedDecisionSource = readFileSync(protectedDecisionPath, 'utf8');
const serverAuthSource = readFileSync(serverAuthPath, 'utf8');

const violations = [];

if (!protectedDecisionSource.includes('resolveAuthVerificationPolicy')) {
 violations.push('Protected route decision must resolve auth verifier policy.');
}

if (!protectedDecisionSource.includes("reason: 'access_token_invalid_missing_verifier_config'")) {
 violations.push('Protected route decision must fail closed when verifier config is missing.');
}

if (!serverAuthSource.includes('resolveAuthVerificationPolicy')) {
 violations.push('Server auth must resolve verifier policy before handling missing verifier config.');
}

if (!/if \(verification\.reason === 'missing_verifier_config'\)[\s\S]*return undefined;/.test(serverAuthSource)) {
 violations.push('Server auth missing-verifier flow must include fail-closed return path.');
}

if (violations.length > 0) {
 console.error('Auth fail-closed guard failed.');
 for (const violation of violations) {
  console.error(`- ${violation}`);
 }
 process.exit(1);
}

console.log('Auth fail-closed guard passed.');
