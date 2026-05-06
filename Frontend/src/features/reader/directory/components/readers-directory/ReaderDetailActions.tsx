import { Loader2, MessageCircle } from 'lucide-react';
import { Button } from '@/shared/ui';
import { cn } from '@/lib/utils';

interface ReaderDetailActionsProps {
 isStartingConversation: boolean;
 ctaLabel: string;
 onStartConversation: () => Promise<void>;
}

export function ReaderDetailActions({
 isStartingConversation,
 ctaLabel,
 onStartConversation,
}: ReaderDetailActionsProps) {
 return (
  <div className={cn('flex justify-end border-t pt-6 tn-border-soft')}>
   <Button onClick={onStartConversation} disabled={isStartingConversation} className={cn('w-full px-6 py-3 font-bold sm:w-auto')}>
    {isStartingConversation ? <Loader2 className={cn('mr-2 h-4 w-4 animate-spin')} /> : <MessageCircle className={cn('mr-2 h-4 w-4')} />}
    {ctaLabel}
   </Button>
  </div>
 );
}
