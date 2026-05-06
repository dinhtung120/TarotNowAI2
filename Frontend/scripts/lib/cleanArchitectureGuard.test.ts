import { describe, expect, it } from 'vitest';
import {
 findClientBoundaryViolation,
 findForbiddenFeatureLayerFolder,
 findForbiddenSharedLayerFolder,
 findSensitiveStreamViolation,
 findUnclassifiedRuntimeFiles,
 isAllowedSharedFeatureImport,
 isSharedComponentsPath,
 isSharedImportingFeature,
 resolveLayer,
} from './cleanArchitectureGuard.mjs';

describe('cleanArchitectureGuard', () => {
 it('classifies runtime files across app, features, shared, and proxy boundaries', () => {
  expect(resolveLayer('src/app/[locale]/page.tsx')).toBe('presentation');
  expect(resolveLayer('src/proxy.ts')).toBe('presentation');
  expect(resolveLayer('src/features/community/post/components/PostComposer.tsx')).toBe('application');
  expect(resolveLayer('src/features/community/feed/hooks/useFeed.ts')).toBe('application');
  expect(resolveLayer('src/features/admin/system-configs/system-config.types.ts')).toBe('application');
  expect(resolveLayer('src/i18n/routing.tsx')).toBe('application');
  expect(resolveLayer('src/shared/server/auth/redirectAuthenticatedAuthEntry.ts')).toBe('infrastructure');
  expect(resolveLayer('src/shared/actions/followRequest.ts')).toBe('application');
  expect(resolveLayer('src/shared/gateways/domainCommandRegistry.ts')).toBe('application');
  expect(resolveLayer('src/shared/models/actionResult.ts')).toBe('domain');
  expect(resolveLayer('src/shared/auth/serverAuth.ts')).toBe('infrastructure');
  expect(resolveLayer('src/shared/query/queryClient.ts')).toBe('infrastructure');
  expect(resolveLayer('src/shared/config/runtimePolicyFallbacks.ts')).toBe('application');
  expect(resolveLayer('src/shared/ui/Button.tsx')).toBe('presentation');
  expect(resolveLayer('src/shared/app-shell/navigation/navbar/Navbar.tsx')).toBe('presentation');
 });

 it('reports runtime files that still have no layer classification', () => {
  expect(findUnclassifiedRuntimeFiles([
   'src/features/community/post/components/PostComposer.tsx',
   'src/features/community/legacyThing.ts',
   'src/shared/unknownThing.ts',
  ])).toEqual(['src/shared/unknownThing.ts']);
 });

 it('detects direct public API access and sensitive EventSource payloads', () => {
  expect(findClientBoundaryViolation(
   'src/features/example/components/Foo.tsx',
   'const apiBase = getPublicApiBaseUrl();',
  )).toMatchObject({
   line: 1,
   message: expect.stringContaining('getPublicApiBaseUrl'),
  });

  expect(findSensitiveStreamViolation(
   'src/features/reading/session/hooks/useFoo.ts',
   "const sse = new EventSource(`/api/stream?followupQuestion=secret`);",
  )).toMatchObject({
   line: 1,
   message: expect.stringContaining('follow-up prompt data'),
  });
 });

 describe('frontend folder ownership guard', () => {
  it('flags layer folders inside features', () => {
   expect(findForbiddenFeatureLayerFolder('src/features/admin/users/application/useAdminUsers.ts')).toEqual('application');
   expect(findForbiddenFeatureLayerFolder('src/features/admin/users/presentation/AdminUsersPage.tsx')).toEqual('presentation');
   expect(findForbiddenFeatureLayerFolder('src/features/chat/shared/domain/participantId.ts')).toEqual('domain');
   expect(findForbiddenFeatureLayerFolder('src/features/admin/gamification/infrastructure/adminGamificationServer.ts')).toEqual('infrastructure');
  });

  it('does not flag practical role folders inside features', () => {
   expect(findForbiddenFeatureLayerFolder('src/features/admin/users/hooks/useAdminUsers.ts')).toBeNull();
   expect(findForbiddenFeatureLayerFolder('src/features/admin/users/components/AdminUsersPage.tsx')).toBeNull();
   expect(findForbiddenFeatureLayerFolder('src/features/chat/shared/participantId.ts')).toBeNull();
   expect(findForbiddenFeatureLayerFolder('src/features/admin/gamification/actions/adminGamificationServer.ts')).toBeNull();
  });

  it('flags removed shared layer folders', () => {
   expect(findForbiddenSharedLayerFolder('src/shared/application/hooks/useAuth.ts')).toBe('application');
   expect(findForbiddenSharedLayerFolder('src/shared/components/ui/Button.tsx')).toBe('components');
   expect(findForbiddenSharedLayerFolder('src/shared/domain/actionResult.ts')).toBe('domain');
   expect(findForbiddenSharedLayerFolder('src/shared/infrastructure/auth/serverAuth.ts')).toBe('infrastructure');
   expect(findForbiddenSharedLayerFolder('src/shared/actions/followRequest.ts')).toBeNull();
   expect(findForbiddenSharedLayerFolder('src/shared/models/actionResult.ts')).toBeNull();
   expect(isSharedComponentsPath('src/shared/components/ui/Button.tsx')).toBe(true);
   expect(isSharedComponentsPath('src/shared/ui/Button.tsx')).toBe(false);
   expect(isSharedComponentsPath('src/shared/app-shell/navigation/navbar/Navbar.tsx')).toBe(false);
  });

  it('flags shared source importing features', () => {
   expect(isSharedImportingFeature('src/shared/app-shell/navigation/navbar/NavbarRightSection.tsx', '@/features/wallet/public')).toBe(true);
   expect(isSharedImportingFeature('src/features/wallet/overview/WalletOverviewPage.tsx', '@/features/auth/public')).toBe(false);
  });

  it('allows known app-wide orchestration files while shared ownership is audited', () => {
   expect(isAllowedSharedFeatureImport('src/shared/server/prefetch/runners/user/wallet.ts')).toBe(true);
   expect(isAllowedSharedFeatureImport('src/shared/app-shell/navigation/navbar/NavbarRightSection.tsx')).toBe(true);
   expect(isAllowedSharedFeatureImport('src/shared/ui/Button.tsx')).toBe(false);
  });
 });
});
