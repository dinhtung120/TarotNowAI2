'use client';

import { memo } from 'react';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import { WithdrawAmountSection } from './WithdrawAmountSection';
import { WithdrawBankFields } from './WithdrawBankFields';
import { WithdrawSubmitSection } from './WithdrawSubmitSection';
import type { WithdrawFormCardProps } from './WithdrawFormCard.types';
import { useWithdrawFormCard } from './useWithdrawFormCard';

function WithdrawFormCardComponent(props: WithdrawFormCardProps) {
  const vm = useWithdrawFormCard(props);

  return (
    <GlassCard className={cn('animate-in fade-in slide-in-from-bottom-8 delay-200 duration-1000')}>
      <form className={cn('space-y-6')} onSubmit={vm.submitWithValidation}>
        <WithdrawAmountSection
          amount={vm.watchedAmount}
          amountNum={props.amountNum}
          feeVnd={props.feeVnd}
          grossVnd={props.grossVnd}
          labels={{
            amountLabel: props.labels.amountLabel,
            amountPlaceholder: props.labels.amountPlaceholder,
            grossLabel: props.labels.grossLabel,
            feeLabel: props.labels.feeLabel,
            netLabel: props.labels.netLabel,
          }}
          locale={props.locale}
          netVnd={props.netVnd}
          onAmountChange={vm.setAmount}
        />
        <WithdrawBankFields
          payoutInfo={props.payoutInfo}
          payoutConfigured={props.payoutConfigured}
          profilePath={props.profilePath}
          userNote={vm.watchedUserNote}
          labels={{
            bankInfoTitle: props.labels.bankInfoTitle,
            bankNameLabel: props.labels.bankNameLabel,
            bankBinLabel: props.labels.bankBinLabel,
            accountNameLabel: props.labels.accountNameLabel,
            accountNumberLabel: props.labels.accountNumberLabel,
            bankInfoMissing: props.labels.bankInfoMissing,
            bankInfoUpdateCta: props.labels.bankInfoUpdateCta,
            noteLabel: props.labels.noteLabel,
            notePlaceholder: props.labels.notePlaceholder,
          }}
          onUserNoteChange={vm.setUserNote}
        />
        <WithdrawSubmitSection
          amountNum={props.amountNum}
          error={props.error}
          submitLabel={props.labels.submitLabel}
          submitting={props.submitting}
          submittingLabel={props.labels.submittingLabel}
          success={props.success}
          successMessage={props.labels.successMessage}
        />
      </form>
    </GlassCard>
  );
}

export const WithdrawFormCard = memo(WithdrawFormCardComponent);
