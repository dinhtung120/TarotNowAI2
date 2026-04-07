import { Clock } from 'lucide-react';
import { cn } from '@/lib/utils';

interface HistoryErrorBannerProps {
 message: string;
}

export function HistoryErrorBanner({ message }: HistoryErrorBannerProps) {
 return (
  <div className={cn('mb-8 p-4 bg-[var(--danger)]/10 border border-[var(--danger)]/20 rounded-xl flex items-center gap-3 text-[var(--danger)] text-sm animate-in zoom-in-95')}>
   <Clock className={cn('w-5 h-5 flex-shrink-0')} />
   <p>{message}</p>
  </div>
 );
}
