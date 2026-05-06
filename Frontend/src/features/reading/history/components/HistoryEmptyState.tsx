import { Bot } from 'lucide-react';
import { useOptimizedNavigation } from '@/shared/infrastructure/navigation/useOptimizedNavigation';
import { Button, GlassCard } from '@/shared/ui';
import { cn } from '@/lib/utils';

interface HistoryEmptyStateProps {
 title: string;
 description: string;
 cta: string;
}

export function HistoryEmptyState({ title, description, cta }: HistoryEmptyStateProps) {
 const navigation = useOptimizedNavigation();

 return (
  <GlassCard className={cn('text-center py-24 animate-in fade-in duration-1000')}>
   <Bot className={cn('w-16 h-16 tn-text-tertiary mx-auto mb-6')} />
   <h3 className={cn('text-xl font-bold tn-text-primary mb-2 tracking-tight')}>{title}</h3>
   <p className={cn('tn-text-secondary mb-8 max-w-sm mx-auto')}>{description}</p>
   <Button variant="brand" onClick={() => navigation.push('/reading')}>
    {cta}
   </Button>
  </GlassCard>
 );
}
