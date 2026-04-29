'use client';

import { useCallback, useMemo, useState } from 'react';
import toast from 'react-hot-toast';
import { useLocale, useTranslations } from 'next-intl';
import {
 listPromotions as listPromotionsAction,
 createPromotion as createPromotionAction,
 updatePromotion as updatePromotionAction,
 deletePromotion as deletePromotionAction,
 type DepositPromotion,
} from '@/features/admin/application/actions/promotion';

export function useAdminPromotions(
 initialPromotions: DepositPromotion[],
 initialLoadError: string | null,
) {
 const t = useTranslations('Admin');
 const locale = useLocale();

 const [promotions, setPromotions] = useState<DepositPromotion[]>(initialPromotions);
 const [loading, setLoading] = useState(false);
 const [listError, setListError] = useState(initialLoadError ?? '');
 const [isCreating, setIsCreating] = useState(false);
 const [minAmount, setMinAmount] = useState<number>(0);
 const [bonusGold, setBonusGold] = useState<number>(0);
 const [deleteId, setDeleteId] = useState<string | null>(null);
 const [submitting, setSubmitting] = useState(false);
 const [togglingId, setTogglingId] = useState<string | null>(null);
 const [deletingId, setDeletingId] = useState<string | null>(null);

 const moneyFormatter = useMemo(
  () => new Intl.NumberFormat(locale, { style: 'currency', currency: 'VND', maximumFractionDigits: 0 }),
  [locale]
 );

 const formatMoney = useCallback((value: number) => moneyFormatter.format(value), [moneyFormatter]);

 const refreshPromotions = useCallback(async (options?: { showLoading?: boolean }) => {
  const showLoading = options?.showLoading ?? true;
  if (showLoading) setLoading(true);
  setListError('');

  try {
   const result = await listPromotionsAction(false);
   if (!result.success || !result.data) {
    const message = result.error || t('promotions.toast.network_error');
    setListError(message);
    return;
   }

   setPromotions(result.data);
   setListError('');
  } catch {
   setListError(t('promotions.toast.network_error'));
  } finally {
   if (showLoading) setLoading(false);
  }
 }, [t]);

 const handleCreate = useCallback(async (payload: { minAmount: number; bonusGold: number }) => {
  setSubmitting(true);

  try {
   const result = await createPromotionAction(payload.minAmount, payload.bonusGold);
   if (result.success) {
    setIsCreating(false);
    setMinAmount(0);
    setBonusGold(0);
    toast.success(t('promotions.toast.create_success'));
    await refreshPromotions({ showLoading: false });
   } else {
    toast.error(t('promotions.toast.create_failed'));
   }
  } catch {
   toast.error(t('promotions.toast.network_error'));
  } finally {
   setSubmitting(false);
  }
 }, [refreshPromotions, t]);

 const handleToggle = useCallback(async (promotion: DepositPromotion) => {
  setTogglingId(promotion.id);
  try {
   const nextActive = !promotion.isActive;
   const result = await updatePromotionAction(promotion.id, {
    minAmountVnd: promotion.minAmountVnd,
    bonusGold: promotion.bonusGold,
    isActive: nextActive,
   });
   if (result.success) {
    setPromotions((prev) =>
     prev.map((item) => (item.id === promotion.id ? { ...item, isActive: nextActive } : item))
    );
    toast.success(t('promotions.toast.toggle_success'));
   } else {
    toast.error(t('promotions.toast.toggle_failed'));
   }
  } catch {
   toast.error(t('promotions.toast.network_error'));
  } finally {
   setTogglingId(null);
  }
 }, [t]);

 const handleDelete = useCallback(async () => {
  if (!deleteId) return;

  setDeletingId(deleteId);
  try {
   const result = await deletePromotionAction(deleteId);
   if (result.success) {
    setPromotions((prev) => prev.filter((item) => item.id !== deleteId));
    setDeleteId(null);
    toast.success(t('promotions.toast.delete_success'));
   } else {
    toast.error(t('promotions.toast.delete_failed'));
   }
  } catch {
   toast.error(t('promotions.toast.network_error'));
  } finally {
   setDeletingId(null);
  }
 }, [deleteId, t]);

 const retryList = useCallback(async () => {
  await refreshPromotions();
 }, [refreshPromotions]);

 return {
  t,
  locale,
  promotions,
  loading,
  listError,
  isCreating,
  setIsCreating,
  minAmount,
  setMinAmount,
  bonusGold,
  setBonusGold,
  deleteId,
  setDeleteId,
  submitting,
  togglingId,
  deletingId,
  formatMoney,
  retryList,
  handleCreate,
  handleToggle,
  handleDelete,
 };
}
