'use client';

import { CheckCircle2, Loader2, User, XCircle } from 'lucide-react';
import { Button, Input } from '@/shared/ui';
import { cn } from '@/lib/utils';

interface AdminWithdrawalActionsProps {
 approveLabel: string;
 disabled: boolean;
 notePlaceholder: string;
 onApprove: () => void;
 onChangeNote: (value: string) => void;
 onReject: () => void;
 rejectLabel: string;
 value: string;
}

export function AdminWithdrawalActions({
 approveLabel,
 disabled,
 notePlaceholder,
 onApprove,
 onChangeNote,
 onReject,
 rejectLabel,
 value,
}: AdminWithdrawalActionsProps) {
 return (
  <div className={cn('tn-flex-col-row-md items-center gap-4 pt-2 border-t tn-border-soft')}>
   <Input
    leftIcon={<User className={cn('w-4 h-4')} />}
    placeholder={notePlaceholder}
    value={value}
    onChange={(event) => onChangeNote(event.target.value)}
    className={cn('flex-1 w-full text-xs font-black uppercase tracking-widest tn-text-primary shadow-inner tn-panel-soft')}
   />
   <div className={cn('flex items-center gap-3 w-full tn-w-full-auto-md shrink-0')}>
    <Button variant="primary" onClick={onApprove} disabled={disabled} className={cn('tn-flex-1-none-sm py-3 shadow-md tn-bg-success tn-text-primary tn-hover-brightness-110')}>
     {disabled ? <Loader2 className={cn('w-4 h-4 animate-spin')} /> : <CheckCircle2 className={cn('w-4 h-4')} />}
     {approveLabel}
    </Button>
    <Button variant="danger" onClick={onReject} disabled={disabled} className={cn('tn-flex-1-none-sm py-3 shadow-md')}>
     {disabled ? <Loader2 className={cn('w-4 h-4 animate-spin')} /> : <XCircle className={cn('w-4 h-4')} />}
     {rejectLabel}
    </Button>
   </div>
  </div>
 );
}
