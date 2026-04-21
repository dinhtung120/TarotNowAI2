import { Building2, CreditCard } from 'lucide-react';
import { cn } from '@/lib/utils';
import { WithdrawBankField } from './WithdrawBankField';

interface WithdrawBankFieldsProps {
 bankName: string;
 accountName: string;
 accountNumber: string;
 userNote: string;
 labels: {
  bankLabel: string;
  bankPlaceholder: string;
  accountNameLabel: string;
  accountNamePlaceholder: string;
  accountNumberLabel: string;
  accountNumberPlaceholder: string;
  noteLabel: string;
  notePlaceholder: string;
 };
 onBankNameChange: (value: string) => void;
 onAccountNameChange: (value: string) => void;
 onAccountNumberChange: (value: string) => void;
 onUserNoteChange: (value: string) => void;
}

export function WithdrawBankFields({
 bankName,
 accountName,
 accountNumber,
 userNote,
 labels,
 onBankNameChange,
 onAccountNameChange,
 onAccountNumberChange,
 onUserNoteChange,
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
   <div className={cn('space-y-3')}>
    <label className={cn('tn-text-10 font-black uppercase tn-tracking-02 tn-text-secondary block')}>
     {labels.noteLabel}
    </label>
    <textarea
     value={userNote}
     onChange={(event) => onUserNoteChange(event.target.value)}
     placeholder={labels.notePlaceholder}
     rows={3}
     maxLength={1000}
     className={cn('w-full px-4 py-3 tn-field rounded-xl text-sm tn-text-primary placeholder:tn-text-muted tn-field-accent transition-all resize-y')}
    />
   </div>
  </div>
 );
}
