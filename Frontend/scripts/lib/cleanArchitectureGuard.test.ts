import { describe, expect, it } from 'vitest';
import {
 findClientBoundaryViolation,
 findSensitiveStreamViolation,
 findUnclassifiedRuntimeFiles,
 resolveLayer,
} from './cleanArchitectureGuard.mjs';

describe('cleanArchitectureGuard', () => {
 it('classifies runtime files across app, features, shared, components, store, and proxy boundaries', () => {
  expect(resolveLayer('src/app/[locale]/page.tsx')).toBe('presentation');
  expect(resolveLayer('src/proxy.ts')).toBe('presentation');
  expect(resolveLayer('src/components/ui/gacha/GachaPageClient.tsx')).toBe('presentation');
  expect(resolveLayer('src/features/community/components/PostComposer.tsx')).toBe('presentation');
  expect(resolveLayer('src/features/community/hooks/useFeed.ts')).toBe('application');
  expect(resolveLayer('src/features/admin/system-configs/system-config.types.ts')).toBe('application');
  expect(resolveLayer('src/i18n/routing.tsx')).toBe('application');
  expect(resolveLayer('src/shared/server/auth/redirectAuthenticatedAuthEntry.ts')).toBe('infrastructure');
  expect(resolveLayer('src/shared/config/runtimePolicyFallbacks.ts')).toBe('application');
  expect(resolveLayer('src/store/authStore.ts')).toBe('application');
 });

 it('reports runtime files that still have no layer classification', () => {
  expect(findUnclassifiedRuntimeFiles([
   'src/features/community/components/PostComposer.tsx',
   'src/features/community/legacyThing.ts',
   'src/store/authStore.ts',
  ])).toEqual(['src/features/community/legacyThing.ts']);
 });

 it('detects direct public API access and sensitive EventSource payloads', () => {
  expect(findClientBoundaryViolation(
   'src/features/example/presentation/Foo.tsx',
   'const apiBase = getPublicApiBaseUrl();',
  )).toMatchObject({
   line: 1,
   message: expect.stringContaining('getPublicApiBaseUrl'),
  });

  expect(findSensitiveStreamViolation(
   'src/features/reading/presentation/useFoo.ts',
   "const sse = new EventSource(`/api/stream?followupQuestion=secret`);",
  )).toMatchObject({
   line: 1,
   message: expect.stringContaining('follow-up prompt data'),
  });
 });
});
