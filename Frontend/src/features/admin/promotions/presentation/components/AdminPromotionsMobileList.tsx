'use client';

import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';
import type { AdminPromotionsCommonProps } from './types';
import { AdminPromotionsStateBlock } from './AdminPromotionsStateBlock';
import { AdminPromotionsMobileCard } from './AdminPromotionsMobileCard';

export function AdminPromotionsMobileList({
 promotions,
 loading,
 locale,
 formatMoney,
 togglingId,
 onToggle,
 onDelete,
}: AdminPromotionsCommonProps) {
 const t = useTranslations('Admin');

 return (
  <div className={cn('md:hidden p-4 sm:p-6 space-y-3')}>
   {loading || promotions.length === 0 ? (
    <AdminPromotionsStateBlock loading={loading} />
   ) : (
    promotions.map((promotion) => (
     <AdminPromotionsMobileCard
      key={promotion.id}
      promotion={promotion}
      locale={locale}
      formatMoney={(value) => t('promotions.row.condition_from', { amount: formatMoney(value) })}
      togglingId={togglingId}
      onToggle={onToggle}
      onDelete={onDelete}
      conditionLabel={t('promotions.table.heading_condition')}
      rewardLabel={t('promotions.table.heading_reward')}
      deleteLabel={t('promotions.delete_modal.delete')}
     />
    ))
   )}
  </div>
 );
}
