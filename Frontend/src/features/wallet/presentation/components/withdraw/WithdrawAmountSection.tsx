import { WithdrawAmountField } from './WithdrawAmountField';
import { WithdrawEstimateCard } from './WithdrawEstimateCard';

interface WithdrawAmountSectionProps {
 locale: string;
 amount: string;
 amountNum: number;
 grossVnd: number;
 feeVnd: number;
 netVnd: number;
 labels: {
  amountLabel: string;
  amountPlaceholder: string;
  grossLabel: string;
  feeLabel: string;
  netLabel: string;
 };
 onAmountChange: (value: string) => void;
}

export function WithdrawAmountSection({
 locale,
 amount,
 amountNum,
 grossVnd,
 feeVnd,
 netVnd,
 labels,
 onAmountChange,
}: WithdrawAmountSectionProps) {
 return (
  <>
   <WithdrawAmountField
    label={labels.amountLabel}
    placeholder={labels.amountPlaceholder}
    value={amount}
    onChange={onAmountChange}
   />
   {amountNum >= 50 ? (
    <WithdrawEstimateCard
     locale={locale}
     grossVnd={grossVnd}
     feeVnd={feeVnd}
     netVnd={netVnd}
     labels={{
      gross: labels.grossLabel,
      fee: labels.feeLabel,
      net: labels.netLabel,
     }}
    />
   ) : null}
  </>
 );
}
