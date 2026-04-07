'use client';

import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import type { AdminPromotionsCommonProps } from './types';
import { AdminPromotionsMobileList } from './AdminPromotionsMobileList';
import { AdminPromotionsDesktopTable } from './AdminPromotionsDesktopTable';

export function AdminPromotionsList(props: AdminPromotionsCommonProps) {
 return (
  <GlassCard className={cn('!p-0 !rounded-[2.5rem] overflow-hidden text-left')}>
   <AdminPromotionsMobileList {...props} />
   <AdminPromotionsDesktopTable {...props} />
  </GlassCard>
 );
}
