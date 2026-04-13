'use client';

import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import { WithdrawAmountSection } from './WithdrawAmountSection';
import { WithdrawBankFields } from './WithdrawBankFields';
import { WithdrawSubmitSection } from './WithdrawSubmitSection';
import type { WithdrawFormCardProps } from './WithdrawFormCard.types';
import { useWithdrawFormCard } from './useWithdrawFormCard';

export function WithdrawFormCard(props: WithdrawFormCardProps) {
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
          accountName={vm.watchedAccountName}
          accountNumber={vm.watchedAccountNumber}
          bankName={vm.watchedBankName}
          labels={{
            bankLabel: props.labels.bankLabel,
            bankPlaceholder: props.labels.bankPlaceholder,
            accountNameLabel: props.labels.accountNameLabel,
            accountNamePlaceholder: props.labels.accountNamePlaceholder,
            accountNumberLabel: props.labels.accountNumberLabel,
            accountNumberPlaceholder: props.labels.accountNumberPlaceholder,
          }}
          onAccountNameChange={vm.setAccountName}
          onAccountNumberChange={vm.setAccountNumber}
          onBankNameChange={vm.setBankName}
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
