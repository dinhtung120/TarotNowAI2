'use client';

import { ShieldCheck, Zap } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

function NoticeItem({
 title,
 body,
 accentClass,
}: {
 title: string;
 body: string;
 accentClass: string;
}) {
 return (
  <div className={cn('p-6 rounded-2xl tn-panel-overlay-soft space-y-2 shadow-inner')}>
   <div className={cn('text-[10px] font-black uppercase tracking-widest flex items-center gap-2', accentClass)}>
    <div className={cn('w-1.5 h-1.5 rounded-full animate-pulse', accentClass.replace('text', 'bg'))} />
    {title}
   </div>
   <p className={cn('text-xs text-[var(--text-secondary)] font-medium leading-relaxed text-left')}>{body}</p>
  </div>
 );
}

export function AdminDashboardNoticePanel() {
 const t = useTranslations('Admin');

 return (
  <GlassCard className={cn('lg:col-span-2 relative group overflow-hidden !rounded-[3rem] border-[var(--purple-accent)]/20 !p-10 !bg-gradient-to-br from-[var(--purple-accent)]/10 to-transparent')}>
   <div className={cn('absolute top-0 right-0 p-10 opacity-10 pointer-events-none group-hover:scale-110 transition-transform duration-1000')}>
    <Zap size={200} className={cn('text-[var(--purple-accent)]')} />
   </div>
   <div className={cn('relative z-10 space-y-6')}>
    <div className={cn('flex items-center gap-3')}>
     <div className={cn('w-10 h-10 rounded-xl bg-[var(--purple-accent)]/20 flex items-center justify-center border border-[var(--purple-accent)]/20 shadow-inner')}>
      <ShieldCheck className={cn('w-5 h-5 text-[var(--purple-accent)]')} />
     </div>
     <h2 className={cn('text-2xl font-black tn-text-primary uppercase italic tracking-tighter drop-shadow-md')}>
      {t('dashboard.notice.title')}
     </h2>
    </div>
    <div className={cn('grid grid-cols-1 md:grid-cols-2 gap-6')}>
     <NoticeItem
      title={t('dashboard.notice.reminder_title')}
      body={t('dashboard.notice.reminder_body')}
      accentClass={cn('text-[var(--success)]')}
     />
     <NoticeItem
      title={t('dashboard.notice.announcement_title')}
      body={t('dashboard.notice.announcement_body')}
      accentClass={cn('text-[var(--warning)]')}
     />
    </div>
   </div>
  </GlassCard>
 );
}
