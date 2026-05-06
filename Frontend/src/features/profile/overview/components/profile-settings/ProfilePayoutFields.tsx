import { Landmark, IdCard, UserRound } from 'lucide-react';
import { memo } from 'react';
import type { ProfileSettingsFieldsGridProps } from '@/features/profile/overview/components/profile-settings/types';
import { cn } from '@/lib/utils';

type RegisterType = ProfileSettingsFieldsGridProps['register'];
type ErrorsType = ProfileSettingsFieldsGridProps['errors'];

interface ProfilePayoutFieldsProps {
  errors: ErrorsType;
  register: RegisterType;
  t: ProfileSettingsFieldsGridProps['t'];
  payoutBankOptions: ProfileSettingsFieldsGridProps['payoutBankOptions'];
}

function ProfilePayoutFieldsComponent({
  errors,
  payoutBankOptions,
  register,
  t,
}: ProfilePayoutFieldsProps) {
  return (
    <div className={cn('space-y-4 rounded-2xl border tn-border p-4 tn-surface-muted')}>
      <p className={cn('tn-text-10 font-black uppercase tn-tracking-02 tn-text-secondary')}>
        {t('payout.section_title')}
      </p>
      <label className={cn('space-y-2 block')}>
        <span className={cn('ml-1 flex items-center gap-2 tn-text-10 font-black uppercase tn-tracking-02 tn-text-secondary')}><Landmark className={cn('h-3.5 w-3.5')} />{t('payout.bank_label')}</span>
        <select {...register('payoutBankBin')} className={cn('tn-field tn-field-accent tn-text-primary w-full rounded-xl px-4 py-3.5 text-sm font-medium')}>
          <option value="">{t('payout.bank_placeholder')}</option>
          {payoutBankOptions.map((option) => (
            <option key={option.bankBin} value={option.bankBin}>{option.bankName}</option>
          ))}
        </select>
        {errors.payoutBankBin ? <p className={cn('ml-1 tn-text-10 font-bold tn-text-danger italic')}>{errors.payoutBankBin.message}</p> : null}
      </label>
      <label className={cn('space-y-2 block')}>
        <span className={cn('ml-1 flex items-center gap-2 tn-text-10 font-black uppercase tn-tracking-02 tn-text-secondary')}><IdCard className={cn('h-3.5 w-3.5')} />{t('payout.account_number_label')}</span>
        <input type="text" inputMode="numeric" {...register('payoutBankAccountNumber')} className={cn('tn-field tn-field-accent tn-text-primary w-full rounded-xl px-4 py-3.5 text-sm font-medium')} placeholder={t('payout.account_number_placeholder')} />
        {errors.payoutBankAccountNumber ? <p className={cn('ml-1 tn-text-10 font-bold tn-text-danger italic')}>{errors.payoutBankAccountNumber.message}</p> : null}
      </label>
      <label className={cn('space-y-2 block')}>
        <span className={cn('ml-1 flex items-center gap-2 tn-text-10 font-black uppercase tn-tracking-02 tn-text-secondary')}><UserRound className={cn('h-3.5 w-3.5')} />{t('payout.account_holder_label')}</span>
        <input type="text" {...register('payoutBankAccountHolder')} className={cn('tn-field tn-field-accent tn-text-primary w-full rounded-xl px-4 py-3.5 text-sm font-medium uppercase')} placeholder={t('payout.account_holder_placeholder')} />
        {errors.payoutBankAccountHolder ? <p className={cn('ml-1 tn-text-10 font-bold tn-text-danger italic')}>{errors.payoutBankAccountHolder.message}</p> : null}
      </label>
    </div>
  );
}

export default memo(ProfilePayoutFieldsComponent);
