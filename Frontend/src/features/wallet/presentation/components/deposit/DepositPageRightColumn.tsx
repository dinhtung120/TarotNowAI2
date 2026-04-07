import type { CreateDepositOrderResponse } from '@/features/wallet/application/actions/deposit';
import { cn } from '@/lib/utils';
import { DepositNotesCard } from './DepositNotesCard';
import { DepositSummaryCard } from './DepositSummaryCard';

interface DepositPageRightColumnProps {
 locale: string;
 amountVnd: number;
 baseDiamond: number;
 bonusGold: number;
 submitting: boolean;
 isValid: boolean;
 order: CreateDepositOrderResponse | null;
 error: string | null;
 formatVnd: (value: number) => string;
 onDeposit: () => Promise<void>;
 summaryLabels: {
  title: string;
  submitting: string;
  submit: string;
  securityNote: string;
  valueLabel: string;
  diamondReceiveLabel: string;
  promoBonusLabel: string;
  totalAssetsLabel: string;
  orderReady: string;
  payNow: string;
 };
 notesTitle: string;
 notesItems: [string, string, string];
}

export function DepositPageRightColumn(props: DepositPageRightColumnProps) {
 return (
  <div className={cn('tn-col-span-5-lg space-y-6')}>
   <DepositSummaryCard
    locale={props.locale}
    amountVnd={props.amountVnd}
    baseDiamond={props.baseDiamond}
    bonusGold={props.bonusGold}
    submitting={props.submitting}
    isValid={props.isValid}
    order={props.order}
    error={props.error}
    formatVnd={props.formatVnd}
    onDeposit={props.onDeposit}
    labels={props.summaryLabels}
   />
   <DepositNotesCard title={props.notesTitle} items={props.notesItems} />
  </div>
 );
}
