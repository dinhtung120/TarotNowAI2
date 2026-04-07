export interface DepositPromotionPreview {
  bonusDiamond: number;
}

export interface DepositAmountSelectionCardProps {
  title: string;
  customLabel: string;
  customPlaceholder: string;
  minAmount: number;
  customAmount: string;
  locale: string;
  isCustom: boolean;
  selectedAmount: number;
  presetAmounts: readonly number[];
  exchangeRate: number;
  formatVnd: (value: number) => string;
  bonusLabel: (amount: number) => string;
  onCustomFocus: () => void;
  onCustomChange: (value: string) => void;
  onSelectPreset: (value: number) => void;
  getPromotion: (value: number) => DepositPromotionPreview | null;
}
