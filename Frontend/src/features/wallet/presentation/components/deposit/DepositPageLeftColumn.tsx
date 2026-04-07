import type { WalletBalance } from '@/features/wallet/domain/types';
import { cn } from '@/lib/utils';
import { DepositAmountSelectionCard } from './DepositAmountSelectionCard';
import { DepositBalanceCard } from './DepositBalanceCard';

interface DepositPageLeftColumnProps {
 balance: WalletBalance | null;
 locale: string;
 exchangeRate: number;
 balanceLabel: string;
 selectionTitle: string;
 customLabel: string;
 customPlaceholder: string;
 minAmount: number;
 customAmount: string;
 isCustom: boolean;
 selectedAmount: number;
 presetAmounts: readonly number[];
 formatVnd: (value: number) => string;
 bonusLabel: (amount: number) => string;
 onCustomFocus: () => void;
 onCustomChange: (value: string) => void;
 onSelectPreset: (value: number) => void;
 getPromotion: (value: number) => { bonusDiamond: number } | null;
}

export function DepositPageLeftColumn(props: DepositPageLeftColumnProps) {
 return (
  <div className={cn('lg:col-span-7 space-y-6')}>
   <DepositBalanceCard
    balance={props.balance}
    locale={props.locale}
    exchangeRate={props.exchangeRate}
    balanceLabel={props.balanceLabel}
   />
   <DepositAmountSelectionCard
    title={props.selectionTitle}
    customLabel={props.customLabel}
    customPlaceholder={props.customPlaceholder}
    minAmount={props.minAmount}
    customAmount={props.customAmount}
    locale={props.locale}
    isCustom={props.isCustom}
    selectedAmount={props.selectedAmount}
    presetAmounts={props.presetAmounts}
    exchangeRate={props.exchangeRate}
    formatVnd={props.formatVnd}
    bonusLabel={props.bonusLabel}
    onCustomFocus={props.onCustomFocus}
    onCustomChange={props.onCustomChange}
    onSelectPreset={props.onSelectPreset}
    getPromotion={props.getPromotion}
   />
  </div>
 );
}
