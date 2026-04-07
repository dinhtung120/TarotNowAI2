import { getTranslations } from 'next-intl/server';
import { SubscriptionPlansList } from '@/features/subscription/components/SubscriptionPlansList';
import { cn } from '@/lib/utils';

export default async function PremiumPage() {
  const t = await getTranslations('Subscription');

  return (
    <div className={cn("max-w-6xl mx-auto py-12 tn-page-pad-4-6-8")}>
      <div className={cn("text-center mb-16")}>
        <h1 className={cn("tn-text-4-5-md font-extrabold text-transparent bg-clip-text tn-premium-gold-gradient mb-4")}>
          {t('premiumTitle', { fallback: 'TarotNow Premium' })}
        </h1>
        <p className={cn("text-lg text-slate-400 max-w-2xl mx-auto")}>
          {t('premiumSubtitle', { fallback: 'Unlock unlimited readings, daily free spreads, and deep AI follow-ups to explore your destiny.' })}
        </p>
      </div>

      <SubscriptionPlansList />
    </div>
  );
}
