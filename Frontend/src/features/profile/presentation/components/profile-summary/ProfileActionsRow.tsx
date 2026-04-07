'use client';

import { ShieldCheck } from 'lucide-react';
import ReaderBusyToggle from '@/features/profile/presentation/ReaderBusyToggle';
import { cn } from '@/lib/utils';

interface ProfileActionsRowProps {
 adminPortalLabel: string;
 isAdmin: boolean;
 isTarotReader: boolean;
 onOpenAdminPortal: () => void;
}

export function ProfileActionsRow({
 adminPortalLabel,
 isAdmin,
 isTarotReader,
 onOpenAdminPortal,
}: ProfileActionsRowProps) {
 return (
  <div className={cn('flex flex-col sm:flex-row gap-3 pt-2')}>
   {isAdmin ? (
    <button
     type="button"
     onClick={onOpenAdminPortal}
     className={cn('flex-1 group flex justify-center items-center gap-2.5 bg-[var(--purple-accent)]/10 hover:bg-[var(--purple-accent)]/20 border border-[var(--purple-accent)]/30 text-[var(--purple-accent)] px-5 py-2.5 min-h-11 rounded-xl text-[10px] font-black uppercase tracking-widest transition-all hover:scale-[1.02] active:scale-95')}
    >
     <ShieldCheck className={cn('w-3.5 h-3.5 transition-transform group-hover:rotate-12')} />
     {adminPortalLabel}
    </button>
   ) : null}
   {isTarotReader ? <ReaderBusyToggle /> : null}
  </div>
 );
}
