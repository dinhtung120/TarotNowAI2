import { Zap } from 'lucide-react';
import { Button } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface DepositSubmitActionProps {
 submitting: boolean;
 isValid: boolean;
 submittingLabel: string;
 submitLabel: string;
 securityNote: string;
 onDeposit: () => Promise<void>;
}

export function DepositSubmitAction({
 submitting,
 isValid,
 submittingLabel,
 submitLabel,
 securityNote,
 onDeposit,
}: DepositSubmitActionProps) {
 return (
  <>
   <Button
    variant="brand"
    fullWidth
    size="lg"
    onClick={() => void onDeposit()}
    disabled={!isValid || submitting}
    isLoading={submitting}
    leftIcon={!submitting ? <Zap className={cn('w-4 h-4')} /> : undefined}
   >
    {submitting ? submittingLabel : submitLabel}
   </Button>
   <p className={cn('text-[9px] font-black uppercase tracking-widest tn-text-muted text-center leading-relaxed')}>
    {securityNote}
   </p>
  </>
 );
}
