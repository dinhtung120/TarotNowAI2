'use client';

import { Clock, Loader2, MessageCircle } from 'lucide-react';
import { Button } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface ReaderProfileFooterProps {
 ctaLabel: string;
 memberSinceLabel: string;
 onStartChat: () => void;
 startingChat: boolean;
}

export function ReaderProfileFooter({
 ctaLabel,
 memberSinceLabel,
 onStartChat,
 startingChat,
}: ReaderProfileFooterProps) {
 return (
  <div className={cn('text-center pt-8')}>
   <Button onClick={onStartChat} disabled={startingChat} className={cn('mb-4 px-6 py-3')}>
    {startingChat ? <Loader2 className={cn('w-4 h-4 animate-spin mr-2')} /> : <MessageCircle className={cn('w-4 h-4 mr-2')} />}
    {ctaLabel}
   </Button>
   <div className={cn('inline-flex items-center gap-2 px-4 py-2 rounded-full tn-panel text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-tertiary)] shadow-inner')}>
    <Clock className={cn('w-3 h-3 text-[var(--text-tertiary)]')} />
    {memberSinceLabel}
   </div>
  </div>
 );
}
