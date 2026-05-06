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
    'tn-core-feature-card group relative flex flex-col gap-10 transition-all duration-700',
    item.hoverClass,
    !isLast && 'tn-core-feature-divider'
   )}
  >
   <div className={cn('tn-core-feature-icon-shell w-14 h-14 rounded-2xl flex items-center justify-center border tn-border transition-all duration-700 animate-glow-pulse')}>
    <item.icon className={cn('tn-core-feature-icon w-6 h-6 transition-colors')} />
   </div>
   <div className={cn('space-y-4')}>
    <h3 className={cn('text-xl font-black tn-text-ink italic tracking-tighter uppercase')}>{item.title}</h3>
    <p className={cn('tn-core-feature-text tn-text-secondary text-sm leading-relaxed font-semibold transition-colors duration-500')}>
     {item.desc}
    </p>
   </div>
  </div>
 );
}

export async function CoreFeaturesSection() {
 const t = await getTranslations('Index');
 const features: FeatureItem[] = [
  { icon: Zap, title: t('f1Title'), desc: t('f1Desc'), hoverClass: 'tn-core-feature-hover-purple' },
  { icon: Flame, title: t('f2Title'), desc: t('f2Desc'), hoverClass: 'tn-core-feature-hover-amber' },
  { icon: ShieldCheck, title: t('f3Title'), desc: t('f3Desc'), hoverClass: 'tn-core-feature-hover-success' },
 ];

 return (
  <section className={cn('relative w-full max-w-6xl mx-auto tn-page-x py-32')}>
   <div className={cn('tn-grid-cols-1-3-md gap-px tn-bg-border-subtle border tn-border tn-rounded-3xl overflow-hidden tn-shadow-elevated')}>
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
