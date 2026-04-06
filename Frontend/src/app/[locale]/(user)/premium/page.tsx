import { getTranslations } from 'next-intl/server';
import { SubscriptionPlansList } from '@/features/subscription/components/SubscriptionPlansList';

export default async function PremiumPage() {
  const t = await getTranslations('Subscription');

  return (
    <div className="max-w-6xl mx-auto py-12 px-4 sm:px-6 lg:px-8">
      <div className="text-center mb-16">
        <h1 className="text-4xl md:text-5xl font-extrabold text-transparent bg-clip-text bg-gradient-to-r from-[#F6D365] to-[#FDA085] mb-4">
          {t('premiumTitle', { fallback: 'TarotNow Premium' })}
        </h1>
        <p className="text-lg text-slate-400 max-w-2xl mx-auto">
          {t('premiumSubtitle', { fallback: 'Unlock unlimited readings, daily free spreads, and deep AI follow-ups to explore your destiny.' })}
        </p>
      </div>

      <SubscriptionPlansList />
    </div>
  );
}
