'use client';

import { useCallback, useEffect, useMemo, useState } from 'react';
import { useMutation, useQuery } from '@tanstack/react-query';
import { useLocale, useTranslations } from 'next-intl';
import {
 createDepositOrder,
 getMyDepositOrder,
 listDepositPackages,
 reconcileDepositOrder,
 type CreateDepositOrderResponse,
 type MyDepositOrderResponse,
} from '@/features/wallet/application/actions/deposit';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import { useWalletStore } from '@/store/walletStore';

type DepositOrderView = MyDepositOrderResponse;

export function useDepositPage() {
 const t = useTranslations('Wallet');
 const locale = useLocale();
 const balance = useWalletStore((state) => state.balance);
 const fetchBalance = useWalletStore((state) => state.fetchBalance);

 const [selectedPackageCode, setSelectedPackageCode] = useState<string>('');
 const [createError, setCreateError] = useState<string | null>(null);
 const [createdOrder, setCreatedOrder] = useState<CreateDepositOrderResponse | null>(null);
 const [orderId, setOrderId] = useState<string | null>(null);

 useQuery({
  queryKey: userStateQueryKeys.wallet.balance(),
  queryFn: async () => {
   await fetchBalance();
   return true;
  },
 });

 const packagesQuery = useQuery({
  queryKey: userStateQueryKeys.wallet.depositPackages(),
  queryFn: async () => {
   const result = await listDepositPackages();
   return result.success && result.data ? result.data : [];
  },
 });

 const effectiveSelectedPackageCode = useMemo(() => {
  return selectedPackageCode || packagesQuery.data?.[0]?.code || '';
 }, [packagesQuery.data, selectedPackageCode]);

 const selectedPackage = useMemo(() => {
  return (packagesQuery.data ?? []).find((item) => item.code === effectiveSelectedPackageCode) ?? null;
 }, [effectiveSelectedPackageCode, packagesQuery.data]);

 const createOrderMutation = useMutation({
  mutationFn: async (packageCode: string) => {
   const idempotencyKey = buildIdempotencyKey(packageCode);
   return createDepositOrder(packageCode, idempotencyKey);
  },
 });

 const orderQuery = useQuery({
  queryKey: userStateQueryKeys.wallet.depositOrder(orderId),
  enabled: Boolean(orderId),
 queryFn: async () => {
  if (!orderId) return null;

   await reconcileDepositOrder(orderId);
   const result = await getMyDepositOrder(orderId);
   if (!result.success || !result.data) {
    throw new Error(result.error || 'Failed to get deposit order');
   }

   return result.data;
  },
  refetchInterval: (query) => {
   const data = query.state.data as MyDepositOrderResponse | null | undefined;
   return data?.status === 'pending' ? 10_000 : false;
  },
 });

 useEffect(() => {
  if (orderQuery.data?.status !== 'success') return;
  void fetchBalance();
 }, [fetchBalance, orderQuery.data?.status]);

 const handleCreateOrder = useCallback(async () => {
  if (!selectedPackage) {
   setCreateError(t('deposit.errors.package_required'));
   return;
  }

  setCreateError(null);
  setCreatedOrder(null);
  setOrderId(null);

  const result = await createOrderMutation.mutateAsync(selectedPackage.code);
  if (!result.success || !result.data) {
   setCreateError(result.error || t('deposit.errors.create_failed'));
   return;
  }

  setCreatedOrder(result.data);
  setOrderId(result.data.orderId);
 }, [createOrderMutation, selectedPackage, t]);

 const activeOrder = useMemo<DepositOrderView | null>(() => {
  if (orderQuery.data) return orderQuery.data;
  if (!createdOrder) return null;
  return toOrderView(createdOrder, effectiveSelectedPackageCode);
 }, [createdOrder, effectiveSelectedPackageCode, orderQuery.data]);

 const resolvedCreateError = useMemo(() => {
  if (createError) return createError;
  if (orderQuery.error) return t('deposit.errors.fetch_order_failed');
  return null;
 }, [createError, orderQuery.error, t]);

 return {
  t,
  locale,
  balance,
  packages: packagesQuery.data ?? [],
  loadingPackages: packagesQuery.isLoading,
  selectedPackageCode: effectiveSelectedPackageCode,
  setSelectedPackageCode,
  selectedPackage,
  createError: resolvedCreateError,
  order: activeOrder,
  creatingOrder: createOrderMutation.isPending,
  pollingOrder: orderQuery.isFetching,
  createOrder: handleCreateOrder,
 };
}

function toOrderView(
 createdOrder: CreateDepositOrderResponse,
 packageCode: string,
): DepositOrderView {
 return {
  orderId: createdOrder.orderId,
  status: createdOrder.status,
  packageCode,
  amountVnd: createdOrder.amountVnd,
  baseDiamondAmount: createdOrder.baseDiamondAmount,
  bonusGoldAmount: createdOrder.bonusGoldAmount,
  totalDiamondAmount: createdOrder.totalDiamondAmount,
  payOsOrderCode: createdOrder.payOsOrderCode,
  checkoutUrl: createdOrder.checkoutUrl,
  qrCode: createdOrder.qrCode,
  paymentLinkId: createdOrder.paymentLinkId,
  expiresAtUtc: createdOrder.expiresAtUtc ?? null,
  transactionId: null,
  failureReason: null,
  processedAt: null,
 };
}

function buildIdempotencyKey(packageCode: string): string {
 const randomPart = typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function'
  ? crypto.randomUUID()
  : `${Date.now()}-${Math.random().toString(16).slice(2)}`;
 return `topup-${packageCode}-${randomPart}`;
}
