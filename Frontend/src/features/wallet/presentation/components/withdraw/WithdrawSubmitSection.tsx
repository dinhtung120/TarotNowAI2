import { AlertTriangle, CheckCircle2 } from 'lucide-react';
import { cn } from '@/lib/utils';
import { WithdrawAlertMessage } from './WithdrawAlertMessage';
import { WithdrawSubmitButton } from './WithdrawSubmitButton';

interface WithdrawSubmitSectionProps {
 amountNum: number;
 submitting: boolean;
 error: string | null;
 success: boolean;
 successMessage: string;
 submittingLabel: string;
 submitLabel: string;
 onSubmitDisabled?: boolean;
}

export function WithdrawSubmitSection({
 amountNum,
 submitting,
 error,
 success,
 successMessage,
 submittingLabel,
 submitLabel,
 onSubmitDisabled = false,
}: WithdrawSubmitSectionProps) {
 return (
  <div className={cn('space-y-3')}>
   {error ? (
   <WithdrawAlertMessage
     icon={AlertTriangle}
     message={error}
     className={cn('bg-[var(--danger)]/10 border border-[var(--danger)]/20 text-[var(--danger)]')}
    />
   ) : null}
   {success ? (
    <WithdrawAlertMessage
     icon={CheckCircle2}
     message={successMessage}
     className={cn('bg-[var(--success)]/10 border border-[var(--success)]/20 text-[var(--success)]')}
    />
   ) : null}
   <WithdrawSubmitButton
    submitting={submitting}
    isDisabled={onSubmitDisabled || submitting || amountNum < 50}
    submittingLabel={submittingLabel}
    submitLabel={submitLabel}
   />
  </div>
 );
}
