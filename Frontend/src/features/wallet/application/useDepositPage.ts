'use client';

import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useLocale, useTranslations } from 'next-intl';
import {
 createDepositOrder,
 getMyDepositOrder,
 listDepositPackages,
 reconcileDepositOrder,
 type CreateDepositOrderResponse,
 type MyDepositOrderResponse,
} from '@/features/wallet/application/actions/deposit';
import { useWalletBalanceQuery } from '@/features/wallet/application/useWalletBalanceQuery';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';
import { queryFnOrThrow } from '@/shared/application/utils/queryPolicy';

type DepositOrderView = MyDepositOrderResponse;
type CreateOrderPayload = { packageCode: string; idempotencyKey: string };
type CreateOrderIntent = { packageCode: string; idempotencyKey: string };

export function useDepositPage() {
 const t = useTranslations('Wallet');
 const locale = useLocale();
 const queryClient = useQueryClient();
 const balanceQuery = useWalletBalanceQuery();

 const [selectedPackageCode, setSelectedPackageCode] = useState<string>('');
 const [createError, setCreateError] = useState<string | null>(null);
 const [createdOrder, setCreatedOrder] = useState<CreateDepositOrderResponse | null>(null);
 const [orderId, setOrderId] = useState<string | null>(null);
 const createOrderIntentRef = useRef<CreateOrderIntent | null>(null);

 const packagesQuery = useQuery({
  queryKey: userStateQueryKeys.wallet.depositPackages(),
  queryFn: async () => {
   const result = await listDepositPackages();
   return queryFnOrThrow(result, 'Failed to list deposit packages');
  },
 });

 const effectiveSelectedPackageCode = useMemo(() => {
  return selectedPackageCode || packagesQuery.data?.[0]?.code || '';
 }, [packagesQuery.data, selectedPackageCode]);

 const selectedPackage = useMemo(() => {
  return (packagesQuery.data ?? []).find((item) => item.code === effectiveSelectedPackageCode) ?? null;
 }, [effectiveSelectedPackageCode, packagesQuery.data]);

 const createOrderMutation = useMutation({
  mutationFn: async (payload: CreateOrderPayload) => {
   return createDepositOrder(payload.packageCode, payload.idempotencyKey);
  },
 });

 const reconcileMutation = useMutation({
  mutationFn: async (currentOrderId: string) => {
   const result = await reconcileDepositOrder(currentOrderId);
   if (!result.success) {
    throw new Error(result.error || 'Failed to reconcile deposit order');
   }
   return result.data ?? false;
  },
 });

 const lastReconcileMarkerRef = useRef<string | null>(null);

 const orderQuery = useQuery({
  queryKey: userStateQueryKeys.wallet.depositOrder(orderId),
  enabled: Boolean(orderId),
  queryFn: async () => {
   if (!orderId) return null;

   const result = await getMyDepositOrder(orderId);
   return queryFnOrThrow(result, 'Failed to get deposit order');
  },
  refetchInterval: (query) => {
   const data = query.state.data as MyDepositOrderResponse | null | undefined;
   return data?.status === 'pending' ? 10_000 : false;
  },
 });

 useEffect(() => {
  if (!orderId || orderQuery.data?.status !== 'pending') {
   return;
  }

  const marker = `${orderId}:${orderQuery.dataUpdatedAt}`;
  if (lastReconcileMarkerRef.current === marker) {
   return;
  }

  lastReconcileMarkerRef.current = marker;
  void reconcileMutation.mutateAsync(orderId).catch(() => undefined);
 }, [orderId, orderQuery.data?.status, orderQuery.dataUpdatedAt, reconcileMutation]);

 useEffect(() => {
  if (orderQuery.data?.status !== 'success') return;
  void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.wallet.balance() });
 }, [orderQuery.data?.status, queryClient]);

 const handleCreateOrder = useCallback(async () => {
  if (!selectedPackage) {
   setCreateError(t('deposit.errors.package_required'));
   return;
  }

  setCreateError(null);
  setCreatedOrder(null);
  setOrderId(null);

  const idempotencyKey = resolveIntentIdempotencyKey(createOrderIntentRef.current, selectedPackage.code);
  createOrderIntentRef.current = { packageCode: selectedPackage.code, idempotencyKey };

  const result = await createOrderMutation.mutateAsync({
   packageCode: selectedPackage.code,
   idempotencyKey,
  });
  if (!result.success || !result.data) {
   setCreateError(result.error || t('deposit.errors.create_failed'));
   return;
  }

  setCreatedOrder(result.data);
  setOrderId(result.data.orderId);
  createOrderIntentRef.current = null;
 }, [createOrderMutation, selectedPackage, t]);

 const handleSelectPackageCode = useCallback((code: string) => {
  setSelectedPackageCode(code);
  if (createOrderIntentRef.current?.packageCode !== code) {
   createOrderIntentRef.current = null;
  }
 }, []);

 const activeOrder = useMemo<DepositOrderView | null>(() => {
  if (orderQuery.data) return orderQuery.data;
  if (!createdOrder) return null;
  return toOrderView(createdOrder, effectiveSelectedPackageCode);
 }, [createdOrder, effectiveSelectedPackageCode, orderQuery.data]);

 const resolvedCreateError = useMemo(() => {
  if (createError) return createError;
  if (packagesQuery.error) return t('deposit.errors.fetch_order_failed');
  if (orderQuery.error) return t('deposit.errors.fetch_order_failed');
  return null;
 }, [createError, orderQuery.error, packagesQuery.error, t]);

 return {
  t,
  locale,
  balance: balanceQuery.data ?? null,
  packages: packagesQuery.data ?? [],
  loadingPackages: packagesQuery.isLoading,
  selectedPackageCode: effectiveSelectedPackageCode,
  setSelectedPackageCode: handleSelectPackageCode,
  selectedPackage,
  createError: resolvedCreateError,
  order: activeOrder,
  creatingOrder: createOrderMutation.isPending,
  pollingOrder: orderQuery.isFetching || reconcileMutation.isPending,
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

function resolveIntentIdempotencyKey(
 currentIntent: CreateOrderIntent | null,
 packageCode: string,
): string {
 if (currentIntent?.packageCode === packageCode) {
  return currentIntent.idempotencyKey;
 }

 return buildIdempotencyKey(packageCode);
}
