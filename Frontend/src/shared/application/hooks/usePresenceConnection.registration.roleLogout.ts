import type { QueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/features/auth/session/authStore';
import { routing } from '@/i18n/routing';
import { performClientLogoutCleanup } from '@/shared/application/gateways/clientLogoutCleanup';

const ROLE_CHANGED_FORCE_LOGOUT_NOTIFICATION_TYPE = 'reader_request_approved';

function resolveLoginPathname(currentPathname: string): string {
 const segments = currentPathname.split('/').filter(Boolean);
 const firstSegment = segments[0] ?? '';
 const hasLocalePrefix = routing.locales.some((locale) => locale === firstSegment);
 const locale = hasLocalePrefix ? firstSegment : routing.defaultLocale;
 return `/${locale}/login`;
}

function shouldForceLogoutFromNotification(notificationType?: string): boolean {
 return notificationType?.trim().toLowerCase() === ROLE_CHANGED_FORCE_LOGOUT_NOTIFICATION_TYPE;
}

export interface PresenceRoleChangeLogoutCoordinator {
 handleNotificationType: (notificationType?: string) => Promise<void>;
}

export function createPresenceRoleChangeLogoutCoordinator(
 queryClient: QueryClient,
): PresenceRoleChangeLogoutCoordinator {
 let roleChangeLogoutTriggered = false;

 const forceLogoutAfterRoleChange = async () => {
  if (roleChangeLogoutTriggered) {
   return;
  }

  roleChangeLogoutTriggered = true;
  try {
   await fetch('/api/auth/logout', {
    method: 'POST',
    credentials: 'include',
    cache: 'no-store',
   });
  } catch {
   // No-op: vẫn cưỡng bức clear session cục bộ để chặn tiếp tục dùng role cũ.
  } finally {
   performClientLogoutCleanup(queryClient);
  }

  if (typeof window === 'undefined') {
   return;
  }

  const loginPath = resolveLoginPathname(window.location.pathname);
  if (window.location.pathname !== loginPath) {
   window.location.assign(loginPath);
  }
 };

 return {
  handleNotificationType: async (notificationType?: string) => {
   if (!shouldForceLogoutFromNotification(notificationType) || !useAuthStore.getState().isAuthenticated) {
    return;
   }

   await forceLogoutAfterRoleChange();
  },
 };
}
