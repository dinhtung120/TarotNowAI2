import { ArrowRight, Loader2 } from 'lucide-react';
import { Button } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface WithdrawSubmitButtonProps {
 submitting: boolean;
 isDisabled: boolean;
 submittingLabel: string;
 submitLabel: string;
}

export function WithdrawSubmitButton({
 submitting,
 isDisabled,
 submittingLabel,
 submitLabel,
}: WithdrawSubmitButtonProps) {
 return (
  <Button variant="primary" type="submit" disabled={isDisabled} className={cn('w-full h-14')}>
   {submitting ? (
    <>
     <Loader2 className={cn('w-4 h-4 animate-spin mr-2')} />
     {submittingLabel}
    </>
   ) : (
    <>
     <ArrowRight className={cn('w-4 h-4 mr-2')} />
     {submitLabel}
    </>
   )}
  </Button>
 );
}
