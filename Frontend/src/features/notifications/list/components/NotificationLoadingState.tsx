import { Bell } from 'lucide-react';
import { GlassCard } from '@/shared/ui';
import { cn } from '@/lib/utils';

interface NotificationLoadingStateProps {
 label: string;
}

export function NotificationLoadingState({ label }: NotificationLoadingStateProps) {
 return (
  <GlassCard className={cn('p-8 text-center')}>
   <div className={cn('flex items-center justify-center gap-3 tn-text-muted')}>
    <Bell className={cn('w-5 h-5 animate-pulse')} />
    <span className={cn('text-sm font-medium')}>{label}</span>
   </div>
  </GlassCard>
 );
}
