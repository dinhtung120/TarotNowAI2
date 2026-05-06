import { normalizeReaderStatus } from '@/features/reader/shared/readerStatus';
import { cn } from '@/lib/utils';

interface ReaderStatusIndicatorProps {
 status: string;
 labels: {
  online: string;
  busy: string;
  offline: string;
 };
}

const baseClass = 'flex items-center gap-1.5';

export function ReaderStatusIndicator({ status, labels }: ReaderStatusIndicatorProps) {
 const normalizedStatus = normalizeReaderStatus(status);

 if (normalizedStatus === 'online') {
  return (
   <div className={cn(baseClass)}>
    <div className={cn('w-2 h-2 rounded-full tn-bg-success animate-pulse')} />
    <span className={cn('tn-text-success font-black uppercase tracking-wider tn-text-9')}>{labels.online}</span>
   </div>
  );
 }

 if (normalizedStatus === 'busy') {
  return (
   <div className={cn(baseClass)}>
    <div className={cn('w-2 h-2 rounded-full tn-bg-warning')} />
    <span className={cn('tn-text-warning font-black uppercase tracking-wider tn-text-9')}>{labels.busy}</span>
   </div>
  );
 }

 return (
  <div className={cn(baseClass)}>
   <div className={cn('w-2 h-2 rounded-full tn-bg-muted')} />
   <span className={cn('tn-text-muted font-bold uppercase tracking-wider tn-text-9')}>{labels.offline}</span>
  </div>
 );
}
