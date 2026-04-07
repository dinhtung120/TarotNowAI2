'use client';

import React from 'react';
import { useTranslations } from 'next-intl';
import { GlassCard } from '@/shared/components/ui';
import { Sparkles, Crown } from 'lucide-react';
import { useMyEntitlements } from '@/features/subscription/hooks/useSubscriptions';
import { cn } from '@/lib/utils';

export const EntitlementsWidget = () => {
  const t = useTranslations('Subscription');

  
  const { data: entitlements, isLoading } = useMyEntitlements();

  
  if (isLoading) {
    return (
      <GlassCard className={cn("mb-12 animate-pulse min-h-[100px] flex items-center justify-center")}>
        <Sparkles className={cn("w-5 h-5 text-slate-500 animate-spin")} />
      </GlassCard>
    );
  }

  
  if (!entitlements || entitlements.length === 0) {
    return null;
  }

  return (
    <GlassCard className={cn("mb-12 border-[#F6D365]/30 bg-slate-800/40 relative overflow-hidden group")}>
      {}
      <div className={cn("absolute inset-0 bg-gradient-to-br from-[#F6D365]/10 to-[#FDA085]/10 opacity-50")} />
      
      <div className={cn("relative z-10 flex flex-col md:flex-row items-center justify-between gap-6")}>
        {}
        <div className={cn("flex items-center gap-4")}>
          <div className={cn("w-12 h-12 rounded-xl bg-gradient-to-br from-[#F6D365] to-[#FDA085] flex items-center justify-center shadow-lg")}>
            <Crown className={cn("w-6 h-6 text-slate-900")} />
          </div>
          <div>
            {}
            <h3 className={cn("text-xl font-black text-transparent bg-clip-text bg-gradient-to-r from-[#F6D365] to-[#FDA085] tracking-tight")}>
              {t('premiumEntitlements')}
            </h3>
            {}
            <p className={cn("text-xs text-slate-400 font-medium")}>{t('entitlementSubtitle')}</p>
          </div>
        </div>

        {}
        <div className={cn("grid grid-cols-2 lg:grid-cols-4 gap-4 w-full md:w-auto flex-grow")}>
          {entitlements.map(ent => (
            <div key={ent.entitlementKey} className={cn("bg-slate-900/50 rounded-lg p-3 border border-slate-700/50")}>
              {}
              <div className={cn("text-[10px] uppercase tracking-wider text-slate-400 mb-1 truncate")}>
                {t(`entitlement_${ent.entitlementKey}`, { fallback: ent.entitlementKey })}
              </div>
              {}
              <div className={cn("text-xl font-bold text-white")}>
                {ent.remainingToday} <span className={cn("text-xs text-slate-500 font-normal")}>{t('leftToday')}</span>
              </div>
            </div>
          ))}
        </div>
      </div>
    </GlassCard>
  );
};
