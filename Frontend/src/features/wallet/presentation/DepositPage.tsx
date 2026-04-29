'use client';

import { memo } from 'react';
import { DepositPackagePicker } from '@/features/wallet/presentation/components/deposit/DepositPackagePicker';
import { DepositPaymentPanel } from '@/features/wallet/presentation/components/deposit/DepositPaymentPanel';
import { useDepositPageViewModel } from '@/features/wallet/presentation/useDepositPageViewModel';
import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
import { cn } from '@/lib/utils';

const DepositPage = memo(function DepositPage() {
 const vm = useDepositPageViewModel();

 return (
  <div className={cn('mx-auto w-full max-w-6xl px-4 pb-24 pt-8 sm:px-6')}>
   <div className={cn('rounded-3xl border tn-border-soft bg-[radial-gradient(circle_at_top_left,rgba(245,158,11,0.18),transparent_45%),radial-gradient(circle_at_bottom_right,rgba(59,130,246,0.18),transparent_45%)] p-6 md:p-8')}>
    <p className={cn('tn-text-overline tn-text-secondary')}>{vm.labels.tag}</p>
    <h1 className={cn('mt-2 text-2xl font-black tn-text-primary md:text-4xl')}>{vm.labels.title}</h1>
    <p className={cn('mt-2 max-w-3xl text-sm tn-text-secondary')}>{vm.labels.subtitle}</p>
    <Link
      href={{ pathname: '/wallet/deposit/history' }}
      locale={vm.locale}
      className={cn('mt-4 inline-flex min-h-11 items-center rounded-xl border tn-border-soft px-3 py-2 text-xs font-black uppercase tracking-wider tn-text-secondary hover:tn-text-primary')}
    >
      {vm.labels.historyCta}
    </Link>
   </div>

   <div className={cn('mt-6 grid grid-cols-1 gap-6 xl:grid-cols-[1.1fr_1fr]')}>
    <section className={cn('rounded-3xl border tn-border-soft tn-panel-soft p-5 space-y-5')}>
     <DepositPackagePicker
      title={vm.labels.packageTitle}
      packages={vm.packages}
      selectedCode={vm.selectedPackageCode}
      onSelect={vm.setSelectedPackageCode}
      formatVnd={vm.formatVnd}
      bonusLabel={vm.labels.packageBonus}
     />
     <button
      type="button"
      onClick={vm.createOrder}
      disabled={vm.creatingOrder || !vm.selectedPackage}
      className={cn('tn-btn-primary w-full rounded-2xl px-4 py-3 text-sm font-black uppercase tracking-widest disabled:opacity-60')}
     >
      {vm.creatingOrder ? vm.labels.creating : vm.labels.createOrder}
     </button>
     {vm.createError ? (
      <div className={cn('rounded-2xl border border-red-400/40 bg-red-500/10 p-3 text-sm text-red-200')}>
       <strong>{vm.labels.errorTitle}:</strong> {vm.createError}
      </div>
     ) : null}
    </section>

    <DepositPaymentPanel
      order={vm.order}
      polling={vm.pollingOrder}
      formatVnd={vm.formatVnd}
      statusLabel={vm.statusLabel}
      labels={vm.labels}
    />
   </div>
  </div>
 );
});

export default DepositPage;
