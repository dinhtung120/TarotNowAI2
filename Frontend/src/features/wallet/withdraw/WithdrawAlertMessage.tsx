import type { LucideIcon } from 'lucide-react';
import { cn } from '@/lib/utils';

interface WithdrawAlertMessageProps {
 icon: LucideIcon;
 message: string;
 className: string;
}

export function WithdrawAlertMessage({
 icon: Icon,
 message,
 className,
}: WithdrawAlertMessageProps) {
 return (
  <div className={cn('flex items-center gap-2 p-4 rounded-xl text-xs font-bold uppercase tracking-widest animate-in zoom-in-95', className)}>
   <Icon className={cn('w-4 h-4')} />
   {message}
  </div>
 );
}
