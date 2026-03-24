'use client';

import {
 AlertCircle,
 ArrowLeft,
 Coins,
 ExternalLink,
 Gem,
 Sparkles,
 Zap,
} from 'lucide-react';
import { Button, GlassCard, SectionHeader } from '@/shared/components/ui';
import { useRouter } from '@/i18n/routing';
import { formatCurrency } from '@/shared/utils/format/formatCurrency';
import { useDepositPage } from '@/features/wallet/application/useDepositPage';

export default function DepositPage() {
 const router = useRouter();
 const {
  t,
  locale,
  balance,
  isCustom,
  setIsCustom,
  selectedAmount,
  customAmount,
  setCustomAmount,
  submitting,
  order,
  error,
  amountVnd,
  isValid,
  baseDiamond,
  bonusGold,
  presetAmounts,
  exchangeRate,
  minAmount,
  handleSelectPreset,
  handleDeposit,
  promoForPreset,
  resetOrderState,
 } = useDepositPage();

 const formatVnd = (value: number) => formatCurrency(value, locale);

 return (
  <div className="max-w-5xl mx-auto px-4 sm:px-6 pt-8 pb-32 space-y-10 w-full">
   <button
    onClick={() => router.push('/wallet')}
    className="group flex items-center gap-2 text-[var(--text-secondary)] hover:tn-text-primary transition-colors text-[10px] font-black uppercase tracking-[0.2em] w-fit min-h-11 px-2 rounded-xl hover:tn-surface-soft"
   >
    <ArrowLeft className="w-3.5 h-3.5 transition-transform group-hover:-translate-x-1" />
    {t('deposit.back_to_wallet')}
   </button>

   <SectionHeader
    tag={t('deposit.tag')}
    tagIcon={<Zap className="w-3 h-3 text-[var(--warning)]" />}
    title={t('deposit.title')}
    subtitle={t('deposit.subtitle')}
    className="animate-in fade-in slide-in-from-bottom-4 duration-1000"
   />

   <div className="grid grid-cols-1 lg:grid-cols-12 gap-10">
    <div className="lg:col-span-7 space-y-6">
     <GlassCard className="flex flex-col sm:flex-row sm:items-center justify-between gap-4 sm:gap-6">
      <div className="flex items-center gap-4">
       <div className="w-10 h-10 rounded-xl bg-[var(--purple-accent)]/10 flex items-center justify-center border border-[var(--purple-accent)]/20">
        <Gem className="w-5 h-5 text-[var(--purple-accent)]" />
       </div>
       <div className="space-y-1">
        <div className="text-[9px] font-black uppercase tracking-widest tn-text-muted">{t('deposit.balance_label')}</div>
        <div className="text-xl font-black tn-text-primary italic">
         {(balance?.diamondBalance ?? 0).toLocaleString(locale)} 💎
        </div>
       </div>
      </div>
      <div className="text-[10px] font-black uppercase tracking-widest tn-text-muted self-start sm:self-auto">
       {exchangeRate.toLocaleString(locale)} VND / 💎
      </div>
     </GlassCard>

     <GlassCard className="space-y-6">
      <h3 className="text-sm font-black tn-text-primary uppercase tracking-widest flex items-center gap-2">
       <Sparkles className="w-4 h-4 text-[var(--warning)]" />
       {t('deposit.select_title')}
      </h3>

      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-3">
       {presetAmounts.map((value) => {
        const selected = !isCustom && selectedAmount === value;
        const promo = promoForPreset(value);

        return (
         <button
          key={value}
          onClick={() => handleSelectPreset(value)}
          className={[
           'relative p-4 rounded-2xl border transition-all text-left',
           selected
            ? 'tn-panel-strong border-[var(--border-hover)] shadow-[var(--glow-purple-sm)]'
            : 'tn-panel border-[var(--border-subtle)] hover:border-[var(--border-hover)] hover:bg-[var(--bg-surface-hover)]',
          ].join(' ')}
         >
          <div className="text-xs font-black tn-text-primary">{formatVnd(value)}</div>
          <div className="text-[10px] font-bold tn-text-muted mt-1">
           {Math.floor(value / exchangeRate).toLocaleString(locale)} 💎
          </div>

          {promo && promo.bonusDiamond > 0 && (
           <div className="absolute top-3 right-3 inline-flex items-center gap-1 px-2 py-0.5 rounded-full bg-[var(--warning)]/15 border border-[var(--warning)]/20 text-[8px] font-black uppercase tracking-widest text-[var(--warning)]">
            <Coins className="w-3 h-3" />
            {t('deposit.bonus_gold', { amount: promo.bonusDiamond })}
           </div>
          )}
         </button>
        );
       })}
      </div>

      <div className="pt-2 space-y-2">
       <label className="text-[10px] font-black uppercase tracking-widest tn-text-muted">{t('deposit.custom_label')}</label>
       <input
        type="number"
        value={customAmount}
        onFocus={() => {
         setIsCustom(true);
         resetOrderState();
        }}
        onChange={(event) => {
         setCustomAmount(event.target.value);
         setIsCustom(true);
         resetOrderState();
        }}
        placeholder={t('deposit.custom_placeholder')}
        min={minAmount}
        className="w-full px-4 py-3 tn-field rounded-2xl tn-text-primary placeholder:tn-text-muted"
       />
      </div>
     </GlassCard>
    </div>

    <div className="lg:col-span-5 space-y-6">
     <GlassCard className="space-y-6">
      <h3 className="text-sm font-black tn-text-primary uppercase tracking-widest">{t('deposit.confirm_title')}</h3>

      <div className="space-y-3 text-[11px]">
       <div className="flex items-center justify-between gap-4">
        <span className="tn-text-muted uppercase tracking-widest text-[9px] font-black">{t('deposit.value_label')}</span>
        <span className="tn-text-primary font-black">{formatVnd(amountVnd)}</span>
       </div>

       <div className="flex items-center justify-between gap-4">
        <span className="tn-text-muted uppercase tracking-widest text-[9px] font-black">
         {t('deposit.diamond_receive_label')}
        </span>
        <span className="tn-text-primary font-black">{baseDiamond.toLocaleString(locale)} 💎</span>
       </div>

       <div className="flex items-center justify-between gap-4">
        <span className="tn-text-muted uppercase tracking-widest text-[9px] font-black">
         {t('deposit.promo_bonus_label')}
        </span>
        <span className="text-[var(--warning)] font-black">+{bonusGold.toLocaleString(locale)} 🪙</span>
       </div>

       <div className="pt-3 border-t border-[var(--border-subtle)] flex items-center justify-between gap-4">
        <span className="tn-text-muted uppercase tracking-widest text-[9px] font-black">
         {t('deposit.total_assets_label')}
        </span>
        <span className="tn-text-primary font-black">
         {baseDiamond.toLocaleString(locale)} 💎 {bonusGold > 0 ? `+ ${bonusGold.toLocaleString(locale)} 🪙` : ''}
        </span>
       </div>
      </div>

      {error && (
       <div className="p-4 rounded-2xl bg-[var(--danger)]/10 border border-[var(--danger)]/20 text-[var(--danger)] text-[10px] font-bold uppercase tracking-widest flex items-center gap-2">
        <AlertCircle className="w-4 h-4" />
        {error}
       </div>
      )}

      {order && !error && (
       <div className="p-4 rounded-2xl bg-[var(--success)]/10 border border-[var(--success)]/20 space-y-3">
        <div className="flex items-center gap-2 text-[10px] font-black uppercase tracking-widest text-[var(--success)]">
         <Sparkles className="w-4 h-4" />
         {t('deposit.order_ready')}
        </div>

        {order.paymentUrl && (
         <a
          href={order.paymentUrl}
          target="_blank"
          rel="noopener noreferrer"
          className="w-full inline-flex items-center justify-center gap-2 px-5 py-3 rounded-2xl bg-[var(--success)] text-[var(--text-ink)] text-[11px] font-black uppercase tracking-widest"
         >
          {t('deposit.pay_now')}
          <ExternalLink className="w-4 h-4" />
         </a>
        )}
       </div>
      )}

      <Button
       variant="brand"
       fullWidth
       size="lg"
       onClick={handleDeposit}
       disabled={!isValid || submitting}
       isLoading={submitting}
       leftIcon={!submitting ? <Zap className="w-4 h-4" /> : undefined}
      >
       {submitting ? t('deposit.submitting') : t('deposit.submit')}
      </Button>

      <p className="text-[9px] font-black uppercase tracking-widest tn-text-muted text-center leading-relaxed">
       {t('deposit.security_note')}
      </p>
     </GlassCard>

     <GlassCard className="space-y-3">
      <div className="text-[9px] font-black uppercase tracking-widest tn-text-muted">{t('deposit.notes_title')}</div>
      <ul className="space-y-1 text-[11px] tn-text-secondary font-medium leading-relaxed list-disc list-inside">
       <li>{t('deposit.notes_item1')}</li>
       <li>{t('deposit.notes_item2')}</li>
       <li>{t('deposit.notes_item3')}</li>
      </ul>
     </GlassCard>
    </div>
   </div>
  </div>
 );
}
