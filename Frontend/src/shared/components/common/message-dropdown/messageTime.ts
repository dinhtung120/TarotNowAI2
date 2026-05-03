import type { useTranslations } from 'next-intl';

export function formatMessageRelativeTime(
 isoDate: string | null | undefined,
 t: ReturnType<typeof useTranslations>,
 referenceNowMs: number,
): string {
 if (!isoDate) return t('time.just_now');
 const target = new Date(isoDate).getTime();
 if (!Number.isFinite(target)) return t('time.just_now');

 const now = Number.isFinite(referenceNowMs) ? referenceNowMs : Date.now();
 const diffMs = Math.max(0, now - target);
 const minutes = Math.floor(diffMs / 60_000);
 if (minutes < 1) return t('time.just_now');
 if (minutes < 60) return t('time.minutes_ago', { count: minutes });

 const hours = Math.floor(minutes / 60);
 if (hours < 24) return t('time.hours_ago', { count: hours });
 return t('time.days_ago', { count: Math.floor(hours / 24) });
}
