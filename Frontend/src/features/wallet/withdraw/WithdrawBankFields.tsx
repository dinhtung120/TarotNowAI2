import { Building2, Landmark, UserRound } from 'lucide-react';
import { memo } from 'react';
import { OptimizedLink as Link } from '@/shared/navigation/useOptimizedLink';
import { cn } from '@/lib/utils';

interface WithdrawBankFieldsProps {
 payoutInfo: {
  bankName: string;
  bankBin: string;
  accountNumber: string;
  accountHolder: string;
 } | null;
 payoutConfigured: boolean;
 profilePath: string;
 userNote: string;
 labels: {
  bankInfoTitle: string;
  bankNameLabel: string;
  bankBinLabel: string;
  accountNameLabel: string;
  accountNumberLabel: string;
  bankInfoMissing: string;
  bankInfoUpdateCta: string;
  noteLabel: string;
  notePlaceholder: string;
 };
 onUserNoteChange: (value: string) => void;
}

function WithdrawBankFieldsComponent({
 labels,
 onUserNoteChange,
 payoutConfigured,
 payoutInfo,
 profilePath,
 userNote,
}: WithdrawBankFieldsProps) {
 return (
  <div className={cn('space-y-5 pt-4')}>
   <div className={cn('space-y-3 rounded-2xl border tn-border p-4 tn-surface-muted')}>
    <p className={cn('tn-text-10 font-black uppercase tn-tracking-02 tn-text-secondary')}>{labels.bankInfoTitle}</p>
    {!payoutConfigured ? (
      <div className={cn('space-y-2')}>
        <p className={cn('text-sm tn-text-danger font-semibold')}>{labels.bankInfoMissing}</p>
        <Link href={profilePath} className={cn('inline-flex text-sm font-semibold underline underline-offset-4 tn-text-primary')}>
          {labels.bankInfoUpdateCta}
        </Link>
      </div>
    ) : (
      <div className={cn('grid gap-3 sm:grid-cols-2')}>
        <BankInfoRow icon={Building2} label={labels.bankNameLabel} value={payoutInfo?.bankName ?? '-'} />
        <BankInfoRow icon={Landmark} label={labels.bankBinLabel} value={payoutInfo?.bankBin ?? '-'} />
        <BankInfoRow icon={UserRound} label={labels.accountNameLabel} value={payoutInfo?.accountHolder ?? '-'} />
        <BankInfoRow icon={Landmark} label={labels.accountNumberLabel} value={payoutInfo?.accountNumber ?? '-'} />
      </div>
    )}
   </div>
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

interface BankInfoRowProps {
 icon: typeof Building2;
 label: string;
 value: string;
}

function BankInfoRow({ icon: Icon, label, value }: BankInfoRowProps) {
 return <div className={cn('rounded-xl border tn-border px-3 py-2')}><p className={cn('tn-text-10 uppercase tn-tracking-02 tn-text-secondary font-bold flex items-center gap-1.5')}><Icon className={cn('h-3.5 w-3.5')} />{label}</p><p className={cn('mt-1 text-sm font-semibold tn-text-primary break-all')}>{value}</p></div>;
}

export const WithdrawBankFields = memo(WithdrawBankFieldsComponent);
