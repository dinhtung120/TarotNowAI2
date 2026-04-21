export interface WithdrawFormCardLabels {
  amountLabel: string;
  amountPlaceholder: string;
  grossLabel: string;
  feeLabel: string;
  netLabel: string;
  bankInfoTitle: string;
  bankNameLabel: string;
  bankBinLabel: string;
  accountNameLabel: string;
  accountNumberLabel: string;
  bankInfoMissing: string;
  bankInfoUpdateCta: string;
  noteLabel: string;
  notePlaceholder: string;
  successMessage: string;
  submittingLabel: string;
  submitLabel: string;
}

export interface WithdrawFormCardProps {
  locale: string;
  amount: string;
  amountNum: number;
  grossVnd: number;
  feeVnd: number;
  netVnd: number;
  payoutInfo: {
    bankName: string;
    bankBin: string;
    accountNumber: string;
    accountHolder: string;
  } | null;
  payoutConfigured: boolean;
  profilePath: string;
  userNote: string;
  submitting: boolean;
  success: boolean;
  error: string | null;
  labels: WithdrawFormCardLabels;
  onAmountChange: (value: string) => void;
  onUserNoteChange: (value: string) => void;
  onSubmit: (event: React.FormEvent<HTMLFormElement>) => void;
}
