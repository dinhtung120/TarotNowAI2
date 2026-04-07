import type { CreateDepositOrderResponse } from "@/features/wallet/application/actions/deposit";

export interface DepositSummaryLabels {
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
}

export interface DepositSummaryCardProps {
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
  labels: DepositSummaryLabels;
}
