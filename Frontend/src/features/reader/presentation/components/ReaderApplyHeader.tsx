import { Sparkles } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

export default function ReaderApplyHeader() {
  const t = useTranslations('ReaderApply');

  return (
    <div className={cn('space-y-4 text-center')}>
      <div className={cn('inline-flex items-center gap-2 rounded-full border border-[var(--purple-accent)]/10 bg-[var(--purple-accent)]/5 px-4 py-1.5 text-[9px] font-black uppercase tracking-[0.2em] text-[var(--purple-accent)] shadow-xl')}>
        <Sparkles className={cn('h-3 w-3 text-[var(--purple-accent)]')} />
        {t('header.tag')}
      </div>
      <h1 className={cn('text-4xl font-black uppercase italic tracking-tighter text-[var(--text-primary)] md:text-5xl')}>
        {t('header.title')}
      </h1>
      <p className={cn('mx-auto max-w-lg text-sm font-medium leading-relaxed text-[var(--text-muted)]')}>
        {t('header.subtitle')}
      </p>
    </div>
  );
}
