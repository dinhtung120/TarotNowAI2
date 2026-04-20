'use client';

import { memo } from 'react';
import type { DepositPackageResponse } from '@/features/wallet/application/actions/deposit';
import { cn } from '@/lib/utils';

interface DepositPackagePickerProps {
 packages: DepositPackageResponse[];
 selectedCode: string;
 onSelect: (code: string) => void;
 formatVnd: (value: number) => string;
 bonusLabel: (amount: number) => string;
 title: string;
}

export const DepositPackagePicker = memo(function DepositPackagePicker({
 packages,
 selectedCode,
 onSelect,
 formatVnd,
 bonusLabel,
 title,
}: DepositPackagePickerProps) {
 return (
  <section className={cn('space-y-4')}>
   <h2 className={cn('tn-text-overline tn-text-tertiary')}>{title}</h2>
   <div className={cn('grid grid-cols-1 gap-3 sm:grid-cols-2')}>
    {packages.map((pkg) => {
     const selected = pkg.code === selectedCode;
     return (
      <button
       key={pkg.code}
       type="button"
       onClick={() => onSelect(pkg.code)}
       className={cn(
        'rounded-2xl border p-4 text-left transition-all',
        selected ? 'border-emerald-400/70 bg-emerald-500/10' : 'tn-border-soft tn-panel-soft',
       )}
      >
       <p className={cn('text-base font-black tn-text-primary')}>{formatVnd(pkg.amountVnd)}</p>
       <p className={cn('mt-1 tn-text-10 tn-text-secondary')}>+{pkg.baseDiamondAmount.toLocaleString()} Diamond</p>
       {pkg.bonusGoldAmount > 0 ? (
        <p className={cn('mt-2 text-xs font-black text-amber-300')}>{bonusLabel(pkg.bonusGoldAmount)}</p>
       ) : null}
      </button>
     );
    })}
   </div>
  </section>
 );
});
