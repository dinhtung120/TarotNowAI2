'use client';

import { Loader2, Power } from 'lucide-react';
import { useTranslations } from 'next-intl';
import type { DepositPromotion } from '@/features/admin/promotions/actions';
import { cn } from '@/lib/utils';

interface AdminPromotionStatusButtonProps {
 promotion: DepositPromotion;
 togglingId: string | null;
 fullWidth?: boolean;
 onToggle: (promotion: DepositPromotion) => void;
}

export function AdminPromotionStatusButton({
 promotion,
 togglingId,
 fullWidth = false,
 onToggle,
}: AdminPromotionStatusButtonProps) {
 const t = useTranslations('Admin');

 return (
  <button
   type="button"
   onClick={() => onToggle(promotion)}
   disabled={togglingId === promotion.id}
   className={cn(
    'relative inline-flex items-center justify-center gap-2 px-4 py-2.5 min-h-11 rounded-xl text-[9px] font-black uppercase tracking-widest border transition-all shadow-inner disabled:opacity-60',
    fullWidth && 'flex-1',
    promotion.isActive
     ? 'bg-[var(--warning)]/10 border-[var(--warning)]/30 text-[var(--warning)] shadow-md hover:bg-[var(--warning)]/20'
     : 'tn-panel text-[var(--text-secondary)] hover:tn-surface hover:tn-text-primary'
   )}
  >
   {togglingId === promotion.id ? <Loader2 className={cn('w-3 h-3 animate-spin')} /> : <Power className={cn('w-3 h-3')} />}
   {promotion.isActive ? t('promotions.status.active') : t('promotions.status.inactive')}
  </button>
 );
}
