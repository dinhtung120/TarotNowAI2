import { Flame, ShieldCheck, Zap } from 'lucide-react';
import { getTranslations } from 'next-intl/server';
import { cn } from '@/lib/utils';

interface FeatureItem {
 icon: typeof Zap;
 title: string;
 desc: string;
 hoverClass: string;
}

function FeatureCard({
 item,
 isLast,
}: {
 item: FeatureItem;
 isLast: boolean;
}) {
 return (
  <div
   className={cn(
    'group relative p-8 sm:p-12 flex flex-col gap-10 bg-[var(--bg-glass)] transition-all duration-700',
    item.hoverClass,
    !isLast && 'border-b md:border-b-0 md:border-r border-[var(--border-subtle)]'
   )}
  >
   <div className={cn('w-14 h-14 rounded-2xl bg-[var(--bg-elevated)] flex items-center justify-center border border-[var(--border-default)] transition-all duration-700 group-hover:scale-110 animate-glow-pulse')}>
    <item.icon className={cn('w-6 h-6 group-hover:text-[var(--text-ink)] transition-colors')} />
   </div>
   <div className={cn('space-y-4')}>
    <h3 className={cn('text-xl font-black text-[var(--text-ink)] italic tracking-tighter uppercase')}>{item.title}</h3>
    <p className={cn('text-[var(--text-secondary)] text-sm leading-relaxed font-semibold transition-colors duration-500 group-hover:text-[var(--text-ink)]')}>
     {item.desc}
    </p>
   </div>
  </div>
 );
}

export async function CoreFeaturesSection() {
 const t = await getTranslations('Index');
 const features: FeatureItem[] = [
  { icon: Zap, title: t('f1Title'), desc: t('f1Desc'), hoverClass: 'hover:bg-[var(--purple-50)]' },
  { icon: Flame, title: t('f2Title'), desc: t('f2Desc'), hoverClass: 'hover:bg-[var(--amber-50)]' },
  { icon: ShieldCheck, title: t('f3Title'), desc: t('f3Desc'), hoverClass: 'hover:bg-[var(--success-bg)]' },
 ];

 return (
  <section className={cn('relative w-full max-w-6xl mx-auto px-4 sm:px-6 py-32')}>
   <div className={cn('grid grid-cols-1 md:grid-cols-3 gap-px bg-[var(--border-subtle)] border border-[var(--border-default)] rounded-[3rem] overflow-hidden shadow-[var(--shadow-elevated)]')}>
    {features.map((item, index) => (
     <FeatureCard
      key={item.title}
      item={item}
      isLast={index === features.length - 1}
     />
    ))}
   </div>
  </section>
 );
}
