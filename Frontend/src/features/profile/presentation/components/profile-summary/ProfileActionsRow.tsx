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
  <div className={cn('tn-flex-col-row-sm gap-3 pt-2')}>
   {isAdmin ? (
    <button
     type="button"
     onClick={onOpenAdminPortal}
     className={cn('flex-1 group flex justify-center items-center gap-2.5 tn-admin-portal-btn border px-5 py-2.5 min-h-11 rounded-xl tn-text-10 font-black uppercase tracking-widest')}
    >
     <ShieldCheck className={cn('w-3.5 h-3.5 tn-admin-portal-icon')} />
     {adminPortalLabel}
    </button>
   ) : null}
   {isTarotReader ? <ReaderBusyToggle /> : null}
  </div>
 );
}
