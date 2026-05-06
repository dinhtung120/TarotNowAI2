import { Sparkles } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';

export default function ReaderApplyHeader() {
  const t = useTranslations('ReaderApply');

 return (
  <div className={cn('space-y-4 text-center')}>
   <div className={cn('inline-flex items-center gap-2 rounded-full border tn-border-accent-20 tn-bg-accent-5 px-4 py-1.5 tn-text-9 font-black uppercase tn-tracking-02 tn-text-accent shadow-xl')}>
    <Sparkles className={cn('h-3 w-3 tn-text-accent')} />
    {t('header.tag')}
   </div>
   <h1 className={cn('tn-text-4-5-md font-black uppercase italic tracking-tighter tn-text-primary')}>
    {t('header.title')}
   </h1>
   <p className={cn('mx-auto max-w-lg text-sm font-medium leading-relaxed tn-text-muted')}>
    {t('header.subtitle')}
   </p>
  </div>
  );
}
