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

export function useAdminPromotions(initialPromotions: DepositPromotion[]) {
 const t = useTranslations('Admin');
 const locale = useLocale();

 const [promotions, setPromotions] = useState<DepositPromotion[]>(initialPromotions);
 const [loading, setLoading] = useState(false);
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

  try {
   const result = await listPromotionsAction(false);
   setPromotions(result.success && result.data ? result.data : []);
  } catch {
   setPromotions([]);
  } finally {
   if (showLoading) setLoading(false);
  }
 }, []);

 const handleCreate = useCallback(async (e: React.FormEvent) => {
  e.preventDefault();
  setSubmitting(true);

  try {
   const result = await createPromotionAction(minAmount, bonusGold);
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
 }, [bonusGold, minAmount, refreshPromotions, t]);

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

 return {
  t,
  locale,
  promotions,
  loading,
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
  handleCreate,
  handleToggle,
  handleDelete,
 };
}
