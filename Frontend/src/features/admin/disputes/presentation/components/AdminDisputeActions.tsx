'use client';

import { Loader2 } from 'lucide-react';
import { Button } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface AdminDisputeActionsProps {
 isProcessing: boolean;
 onRefund: () => void;
 onRelease: () => void;
 onSplit: () => void;
 refundLabel: string;
 releaseLabel: string;
 splitLabel: string;
}

export function AdminDisputeActions({
 isProcessing,
 onRefund,
 onRelease,
 onSplit,
 refundLabel,
 releaseLabel,
 splitLabel,
}: AdminDisputeActionsProps) {
 const spinner = isProcessing ? <Loader2 className={cn('w-4 h-4 animate-spin')} /> : null;
 return (
  <div className={cn('flex flex-col sm:flex-row gap-3')}>
   <Button variant="primary" disabled={isProcessing} onClick={onRelease} className={cn('flex-1')}>
    {spinner}
    {releaseLabel}
   </Button>
   <Button variant="secondary" disabled={isProcessing} onClick={onRefund} className={cn('flex-1')}>
    {spinner}
    {refundLabel}
   </Button>
   <Button variant="secondary" disabled={isProcessing} onClick={onSplit} className={cn('flex-1')}>
    {spinner}
    {splitLabel}
   </Button>
  </div>
 );
}
