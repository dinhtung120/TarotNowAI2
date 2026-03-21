'use client';

import { useEffect, useMemo, useState } from 'react';
import {
 createDepositOrder,
 type CreateDepositOrderResponse,
} from '@/actions/depositActions';
import {
 listPromotions,
 type DepositPromotion,
} from '@/actions/promotionActions';
import { useWalletStore } from '@/store/walletStore';
import { useLocale, useTranslations } from 'next-intl';
import {
 EXCHANGE_RATE_VND_PER_DIAMOND,
 MIN_DEPOSIT_AMOUNT_VND,
 PRESET_DEPOSIT_AMOUNTS_VND,
} from '@/features/wallet/domain/constants';

export function useDepositPage() {
 const t = useTranslations('Wallet');
 const locale = useLocale();
 const balance = useWalletStore((state) => state.balance);
 const fetchBalance = useWalletStore((state) => state.fetchBalance);

 const [isCustom, setIsCustom] = useState(false);
 const [selectedAmount, setSelectedAmount] = useState<number>(PRESET_DEPOSIT_AMOUNTS_VND[1]);
 const [customAmount, setCustomAmount] = useState('');

 const [promotions, setPromotions] = useState<DepositPromotion[]>([]);
 const [loadingPromos, setLoadingPromos] = useState(true);

 const [submitting, setSubmitting] = useState(false);
 const [order, setOrder] = useState<CreateDepositOrderResponse | null>(null);
 const [error, setError] = useState<string | null>(null);

 useEffect(() => {
  void fetchBalance();

  const fetchPromotions = async () => {
   setLoadingPromos(true);
   const data = await listPromotions(true);
   setPromotions(data ?? []);
   setLoadingPromos(false);
  };

  void fetchPromotions();
 }, [fetchBalance]);

 const amountVnd = isCustom ? parseInt(customAmount, 10) || 0 : selectedAmount;
 const isValid = amountVnd >= MIN_DEPOSIT_AMOUNT_VND;
 const baseDiamond = Math.floor(amountVnd / EXCHANGE_RATE_VND_PER_DIAMOND);

 const bestPromotion = useMemo(() => {
  if (loadingPromos) return null;

  return (
   promotions
    .filter((item) => item.isActive && item.minAmountVnd <= amountVnd)
    .sort((a, b) => b.bonusDiamond - a.bonusDiamond)[0] ?? null
  );
 }, [amountVnd, loadingPromos, promotions]);

 const bonusGold = bestPromotion?.bonusDiamond ?? 0;

 const resetOrderState = () => {
  setOrder(null);
  setError(null);
 };

 const handleSelectPreset = (value: number) => {
  setSelectedAmount(value);
  setIsCustom(false);
  setCustomAmount('');
  resetOrderState();
 };

 const handleDeposit = async () => {
  if (!isValid) {
   setError(t('deposit.error_min_amount'));
   return;
  }

  if (submitting) return;

  setSubmitting(true);
  setError(null);
  setOrder(null);

  try {
   const result = await createDepositOrder(amountVnd);
   if (!result) {
    setError(t('deposit.error_create_failed'));
    return;
   }

   setOrder(result);
   if (result.paymentUrl) {
    window.setTimeout(() => {
     window.open(result.paymentUrl, '_blank');
    }, 800);
   }
  } catch {
   setError(t('deposit.error_generic'));
  } finally {
   setSubmitting(false);
  }
 };

 const promoForPreset = (value: number) =>
  promotions
   .filter((item) => item.isActive && item.minAmountVnd <= value)
   .sort((a, b) => b.bonusDiamond - a.bonusDiamond)[0] ?? null;

 return {
  t,
  locale,
  balance,
  isCustom,
  setIsCustom,
  selectedAmount,
  customAmount,
  setCustomAmount,
  promotions,
  loadingPromos,
  submitting,
  order,
  error,
  amountVnd,
  isValid,
  baseDiamond,
  bonusGold,
  presetAmounts: PRESET_DEPOSIT_AMOUNTS_VND,
  exchangeRate: EXCHANGE_RATE_VND_PER_DIAMOND,
  minAmount: MIN_DEPOSIT_AMOUNT_VND,
  handleSelectPreset,
  handleDeposit,
  promoForPreset,
  resetOrderState,
 };
}
