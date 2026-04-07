export interface WithdrawFormCardLabels {
  amountLabel: string;
  amountPlaceholder: string;
  grossLabel: string;
  feeLabel: string;
  netLabel: string;
  bankLabel: string;
  bankPlaceholder: string;
  accountNameLabel: string;
  accountNamePlaceholder: string;
  accountNumberLabel: string;
  accountNumberPlaceholder: string;
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
  bankName: string;
  accountName: string;
  accountNumber: string;
  submitting: boolean;
  success: boolean;
  error: string | null;
  labels: WithdrawFormCardLabels;
  onAmountChange: (value: string) => void;
  onBankNameChange: (value: string) => void;
  onAccountNameChange: (value: string) => void;
  onAccountNumberChange: (value: string) => void;
  onSubmit: (event: React.FormEvent<HTMLFormElement>) => void;
}
