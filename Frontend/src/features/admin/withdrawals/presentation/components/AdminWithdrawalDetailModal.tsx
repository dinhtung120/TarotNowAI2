'use client';

import { memo } from 'react';
import Image from 'next/image';
import Modal from '@/shared/components/ui/Modal';
import type { WithdrawalDetailResult } from '@/features/wallet/public';
import { shouldUseUnoptimizedImage } from '@/shared/infrastructure/http/assetUrl';
import { cn } from '@/lib/utils';

interface AdminWithdrawalDetailModalProps {
  closeLabel: string;
  detail: WithdrawalDetailResult | null;
  detailError: string;
  formatVnd: (value: number | null | undefined) => string;
  loading: boolean;
  onClose: () => void;
  open: boolean;
  title: string;
  transferContentLabel: string;
  qrTitle: string;
  accountHolderLabel: string;
  accountNumberLabel: string;
  bankNameLabel: string;
  amountLabel: string;
}

function AdminWithdrawalDetailModalComponent(props: AdminWithdrawalDetailModalProps) {
  const unoptimizedQrImage = shouldUseUnoptimizedImage(props.detail?.vietQrImageUrl);

  return (
    <Modal isOpen={props.open} onClose={props.onClose} title={props.title} size="lg">
      {props.loading ? <p className={cn('text-sm tn-text-secondary')}>...</p> : null}
      {props.detailError ? <p className={cn('text-sm tn-text-danger font-semibold')}>{props.detailError}</p> : null}
      {!props.loading && !props.detailError && props.detail ? (
        <div className={cn('grid gap-6 md:grid-cols-[1.2fr_1fr]')}>
          <div className={cn('space-y-3')}>
            <DetailRow label={props.amountLabel} value={props.formatVnd(props.detail.netAmountVnd)} />
            <DetailRow label={props.bankNameLabel} value={`${props.detail.bankName} (${props.detail.bankBin})`} />
            <DetailRow label={props.accountNumberLabel} value={props.detail.bankAccountNumber} />
            <DetailRow label={props.accountHolderLabel} value={props.detail.bankAccountName} />
            <DetailRow label={props.transferContentLabel} value={props.detail.transferContent} />
          </div>
          <div className={cn('space-y-2')}>
            <p className={cn('tn-text-10 uppercase tn-tracking-02 font-bold tn-text-secondary')}>{props.qrTitle}</p>
            <Image
              src={props.detail.vietQrImageUrl}
              alt={props.qrTitle}
              width={280}
              height={280}
              className={cn('w-full max-w-[280px] rounded-xl border tn-border bg-white p-2')}
              unoptimized={unoptimizedQrImage}
            />
          </div>
        </div>
      ) : null}
      <div className={cn('mt-6 flex justify-end')}>
        <button type="button" className={cn('rounded-xl px-4 py-2 text-sm font-semibold tn-surface tn-text-primary')} onClick={props.onClose}>
          {props.closeLabel}
        </button>
      </div>
    </Modal>
  );
}

interface DetailRowProps {
  label: string;
  value: string;
}

function DetailRow({ label, value }: DetailRowProps) {
  return <div className={cn('rounded-xl border tn-border px-3 py-2')}><p className={cn('tn-text-10 uppercase tn-tracking-02 font-bold tn-text-secondary')}>{label}</p><p className={cn('mt-1 text-sm font-semibold tn-text-primary break-all')}>{value}</p></div>;
}

export const AdminWithdrawalDetailModal = memo(AdminWithdrawalDetailModalComponent);
