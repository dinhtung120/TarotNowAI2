import type { useTranslations } from 'next-intl';

export function formatRelativeTime(dateStr: string, t: ReturnType<typeof useTranslations>): string {
  const now = Date.now();
  const date = new Date(dateStr).getTime();
  const diffMs = now - date;
  const diffMinutes = Math.floor(diffMs / 60000);
  const diffHours = Math.floor(diffMs / 3600000);
  const diffDays = Math.floor(diffMs / 86400000);

  if (diffMinutes < 1) return t('time_just_now');
  if (diffMinutes < 60) return t('time_minutes_ago', { count: diffMinutes });
  if (diffHours < 24) return t('time_hours_ago', { count: diffHours });
  return t('time_days_ago', { count: diffDays });
}
