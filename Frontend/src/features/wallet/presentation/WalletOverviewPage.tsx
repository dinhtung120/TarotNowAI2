'use client';

import { useWalletOverviewPage } from '@/features/wallet/application/useWalletOverviewPage';
import { EntitlementsWidget } from '@/features/wallet/presentation/components/EntitlementsWidget';
import {
 OverviewHeader,
 WalletBalanceCards,
 WalletLedgerTable,
 WalletLedgerToolbar,
} from '@/features/wallet/presentation/components/overview';
import { cn } from '@/lib/utils';

export default function WalletOverviewPage() {
 const { t, locale, balance, ledger, isLoadingLedger, setPage, formatType } = useWalletOverviewPage();

 return (
  <div className={cn('max-w-5xl mx-auto px-4 sm:px-6 pt-8 pb-32 font-sans relative')}>
   <OverviewHeader
    tag={t('overview.tag')}
    title={t('overview.title')}
    subtitle={t('overview.subtitle')}
    depositCta={t('overview.deposit_cta')}
   />

   <WalletBalanceCards
    balance={balance}
    locale={locale}
    labels={{
     diamondLabel: t('overview.diamond_label'),
     diamondDesc: t('overview.diamond_desc'),
     frozenLabel: (amount) => t('overview.frozen_label', { amount }),
     goldLabel: t('overview.gold_label'),
     goldDesc: t('overview.gold_desc'),
     goldNote: t('overview.gold_note'),
    }}
   />

   <EntitlementsWidget />

   <div className={cn('animate-in fade-in slide-in-from-bottom-12 duration-1000 delay-500')}>
    <WalletLedgerToolbar
     title={t('overview.ledger_title')}
     placeholder={t('overview.search_placeholder')}
    />
    <WalletLedgerTable
     locale={locale}
     ledger={ledger}
     isLoading={isLoadingLedger}
     formatType={formatType}
     onPageChange={setPage}
     labels={{
      tableTime: t('overview.table_time'),
      tableAsset: t('overview.table_asset'),
      tableAction: t('overview.table_action'),
      tableAmount: t('overview.table_amount'),
      ledgerLoading: t('overview.ledger_loading'),
      ledgerEmpty: t('overview.ledger_empty'),
      balanceAfter: (amount) => t('overview.balance_after', { amount }),
     }}
    />
   </div>
  </div>
 );
}
