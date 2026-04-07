'use client';

import { useTranslations } from 'next-intl';
import { cn } from '@/lib/utils';
import type { AdminPromotionsCommonProps } from './types';
import { AdminPromotionsStateBlock } from './AdminPromotionsStateBlock';
import { AdminPromotionsDesktopRow } from './AdminPromotionsDesktopRow';

function AdminPromotionsTableHead() {
 const t = useTranslations('Admin');

 return (
  <thead>
   <tr className={cn('border-b tn-border-soft tn-surface')}>
    <th className={cn('px-8 py-6 tn-text-overline tn-text-tertiary')}>
     {t('promotions.table.heading_condition')}
    </th>
    <th className={cn('px-8 py-6 tn-text-overline tn-text-tertiary')}>
     {t('promotions.table.heading_reward')}
    </th>
    <th className={cn('px-8 py-6 tn-text-overline tn-text-tertiary text-center')}>
     {t('promotions.table.heading_status')}
    </th>
    <th className={cn('px-8 py-6 tn-text-overline tn-text-tertiary text-right')}>
     {t('promotions.table.heading_commands')}
    </th>
   </tr>
  </thead>
 );
}

export function AdminPromotionsDesktopTable({
 promotions,
 loading,
 locale,
 formatMoney,
 togglingId,
 onToggle,
 onDelete,
}: AdminPromotionsCommonProps) {
 return (
  <div className={cn('tn-hide-show-md-block overflow-x-auto custom-scrollbar')}>
   <table className={cn('w-full text-left')}>
    <AdminPromotionsTableHead />
    <tbody className={cn('divide-y divide-white/5')}>
     {loading || promotions.length === 0 ? (
      <tr>
       <td colSpan={4}>
        <AdminPromotionsStateBlock loading={loading} />
       </td>
      </tr>
     ) : (
      promotions.map((promotion) => (
       <AdminPromotionsDesktopRow
        key={promotion.id}
        promotion={promotion}
        locale={locale}
        formatMoney={formatMoney}
        togglingId={togglingId}
        onToggle={onToggle}
        onDelete={onDelete}
       />
      ))
     )}
    </tbody>
   </table>
  </div>
 );
}
