import { Bell, Flame, Info, Star, Wallet, Zap } from 'lucide-react';
import { useTranslations } from 'next-intl';
import type { NotificationItem } from '@/features/notifications/application/actions';

export const notificationTypeIconMap: Record<string, { icon: typeof Bell; colorClass: string }> = {
 system: { icon: Info, colorClass: 'text-blue-400' },
 quest: { icon: Star, colorClass: 'text-purple-400' },
 streak: { icon: Flame, colorClass: 'text-orange-400' },
 escrow: { icon: Wallet, colorClass: 'text-emerald-400' },
 payment: { icon: Zap, colorClass: 'text-yellow-400' },
};

export function formatRelativeNotificationTime(
 dateStr: string,
 t: ReturnType<typeof useTranslations>,
): string {
 const now = Date.now();
 const date = new Date(dateStr).getTime();
 const diffMs = now - date;
 const diffMinutes = Math.floor(diffMs / 60000);
 const diffHours = Math.floor(diffMs / 3600000);
 const diffDays = Math.floor(diffMs / 86400000);

 if (diffMinutes < 1) {
  return t('time_just_now');
 }

 if (diffMinutes < 60) {
  return t('time_minutes_ago', { count: diffMinutes });
 }

 if (diffHours < 24) {
  return t('time_hours_ago', { count: diffHours });
 }

 return t('time_days_ago', { count: diffDays });
}

export function getLocalizedNotificationTitle(item: NotificationItem, locale: string): string {
 if (locale === 'vi') {
  return item.titleVi || item.titleEn;
 }
 return item.titleEn || item.titleVi;
}

export function getLocalizedNotificationBody(item: NotificationItem, locale: string): string {
 if (locale === 'vi') {
  return item.bodyVi || item.bodyEn;
 }
 return item.bodyEn || item.bodyVi;
}
