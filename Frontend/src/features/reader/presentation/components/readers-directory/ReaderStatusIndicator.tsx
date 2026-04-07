import { normalizeReaderStatus } from '@/features/reader/domain/readerStatus';
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
    <div className={cn('w-2 h-2 rounded-full bg-[var(--success)] shadow-[0_0_8px_var(--c-16-185-129-50)] animate-pulse')} />
    <span className={cn('text-[var(--success)] font-black uppercase tracking-wider text-[9px]')}>{labels.online}</span>
   </div>
  );
 }

 if (normalizedStatus === 'busy') {
  return (
   <div className={cn(baseClass)}>
    <div className={cn('w-2 h-2 rounded-full bg-[var(--warning)] shadow-[0_0_8px_var(--c-245-158-11-50)]')} />
    <span className={cn('text-[var(--warning)] font-black uppercase tracking-wider text-[9px]')}>{labels.busy}</span>
   </div>
  );
 }

 return (
  <div className={cn(baseClass)}>
   <div className={cn('w-2 h-2 rounded-full bg-[var(--text-muted)]')} />
   <span className={cn('tn-text-muted font-bold uppercase tracking-wider text-[9px]')}>{labels.offline}</span>
  </div>
 );
}
