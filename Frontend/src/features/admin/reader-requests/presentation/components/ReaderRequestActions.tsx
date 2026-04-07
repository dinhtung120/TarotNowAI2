import { CheckCircle2, Loader2, XCircle } from 'lucide-react';
import { Button, Input } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface ReaderRequestActionsProps {
 requestId: string;
 processingId: string | null;
 note: string;
 notePlaceholder: string;
 approveLabel: string;
 rejectLabel: string;
 onApprove: () => Promise<void>;
 onReject: () => Promise<void>;
 onFocus: () => void;
 onChange: (value: string) => void;
}

export function ReaderRequestActions({
 requestId,
 processingId,
 note,
 notePlaceholder,
 approveLabel,
 rejectLabel,
 onApprove,
 onReject,
 onFocus,
 onChange,
}: ReaderRequestActionsProps) {
 const isProcessing = processingId === requestId;

 return (
  <div className={cn('space-y-4 pt-2')}>
   <Input
    placeholder={notePlaceholder}
    value={note}
    onFocus={onFocus}
    onChange={(event) => onChange(event.target.value)}
    className={cn('w-full text-xs font-black tracking-widest tn-text-primary shadow-inner tn-surface')}
   />
   <div className={cn('flex gap-4')}>
   <Button
    variant="secondary"
    id={`approve-${requestId}`}
    onClick={() => void onApprove()}
    disabled={isProcessing}
    className={cn('flex-1 py-4 tn-btn-success-solid shadow-md')}
   >
     {isProcessing ? <Loader2 className={cn('w-4 h-4 animate-spin')} /> : <CheckCircle2 className={cn('w-4 h-4')} />}
     {approveLabel}
    </Button>
    <Button
    variant="danger"
    id={`reject-${requestId}`}
    onClick={() => void onReject()}
    disabled={isProcessing}
    className={cn('flex-1 py-4 shadow-md tn-btn-danger-soft border')}
   >
     {isProcessing ? <Loader2 className={cn('w-4 h-4 animate-spin')} /> : <XCircle className={cn('w-4 h-4')} />}
     {rejectLabel}
    </Button>
   </div>
  </div>
 );
}
