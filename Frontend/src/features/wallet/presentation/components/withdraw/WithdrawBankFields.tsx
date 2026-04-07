import { Building2, CreditCard } from 'lucide-react';
import { cn } from '@/lib/utils';
import { WithdrawBankField } from './WithdrawBankField';

interface WithdrawBankFieldsProps {
 bankName: string;
 accountName: string;
 accountNumber: string;
 labels: {
  bankLabel: string;
  bankPlaceholder: string;
  accountNameLabel: string;
  accountNamePlaceholder: string;
  accountNumberLabel: string;
  accountNumberPlaceholder: string;
 };
 onBankNameChange: (value: string) => void;
 onAccountNameChange: (value: string) => void;
 onAccountNumberChange: (value: string) => void;
}

export function WithdrawBankFields({
 bankName,
 accountName,
 accountNumber,
 labels,
 onBankNameChange,
 onAccountNameChange,
 onAccountNumberChange,
}: WithdrawBankFieldsProps) {
 return (
  <div className={cn('space-y-5 pt-4')}>
   <WithdrawBankField
    label={labels.bankLabel}
    placeholder={labels.bankPlaceholder}
    value={bankName}
    onChange={onBankNameChange}
    icon={Building2}
   />
   <WithdrawBankField
    label={labels.accountNameLabel}
    placeholder={labels.accountNamePlaceholder}
    value={accountName}
    onChange={onAccountNameChange}
    uppercase
   />
   <WithdrawBankField
    label={labels.accountNumberLabel}
    placeholder={labels.accountNumberPlaceholder}
    value={accountNumber}
    onChange={onAccountNumberChange}
    icon={CreditCard}
   />
  </div>
 );
}
