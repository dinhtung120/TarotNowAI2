import { AlertTriangle, CheckCircle2 } from 'lucide-react';
import { cn } from '@/lib/utils';
import { WithdrawAlertMessage } from './WithdrawAlertMessage';
import { WithdrawSubmitButton } from './WithdrawSubmitButton';

interface WithdrawSubmitSectionProps {
 amountNum: number;
 minWithdrawDiamond: number;
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
 minWithdrawDiamond,
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
     className={cn('tn-bg-danger-soft border tn-border-danger-50 tn-text-danger')}
    />
   ) : null}
   {success ? (
    <WithdrawAlertMessage
     icon={CheckCircle2}
     message={successMessage}
     className={cn('tn-bg-success-10 border tn-border-success-20 tn-text-success')}
    />
   ) : null}
   <WithdrawSubmitButton
    submitting={submitting}
    isDisabled={onSubmitDisabled || submitting || amountNum < minWithdrawDiamond}
    submittingLabel={submittingLabel}
    submitLabel={submitLabel}
   />
  </div>
 );
}
