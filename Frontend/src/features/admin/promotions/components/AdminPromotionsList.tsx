'use client';

import { GlassCard } from '@/shared/ui';
import { cn } from '@/lib/utils';
import type { AdminPromotionsCommonProps } from './types';
import { AdminPromotionsMobileList } from './AdminPromotionsMobileList';
import { AdminPromotionsDesktopTable } from './AdminPromotionsDesktopTable';

export function AdminPromotionsList(props: AdminPromotionsCommonProps) {
 return (
  <GlassCard className={cn('!p-0 !tn-rounded-2_5xl overflow-hidden text-left')}>
   <AdminPromotionsMobileList {...props} />
   <AdminPromotionsDesktopTable {...props} />
  </GlassCard>
 );
}
