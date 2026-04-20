import { useMemo } from 'react';
import { formatCurrency } from '@/shared/utils/format/formatCurrency';
import { useDepositPage } from '@/features/wallet/application/useDepositPage';

export function useDepositPageViewModel() {
 const state = useDepositPage();
 const formatVnd = (value: number) => formatCurrency(value, state.locale);

 const statusLabel = useMemo(() => {
  return {
   pending: state.t('deposit.status.pending'),
   success: state.t('deposit.status.success'),
   failed: state.t('deposit.status.failed'),
  };
 }, [state]);

 return {
  ...state,
  formatVnd,
  statusLabel,
  labels: {
   tag: state.t('deposit.tag'),
   title: state.t('deposit.title'),
   subtitle: state.t('deposit.subtitle'),
   packageTitle: state.t('deposit.package_title'),
   packageBonus: (amount: number) => state.t('deposit.package_bonus', { amount }),
   createOrder: state.t('deposit.create_order'),
   creating: state.t('deposit.creating'),
   notes: state.t('deposit.notes'),
   paymentTitle: state.t('deposit.payment_title'),
   openCheckout: state.t('deposit.open_checkout'),
   qrTitle: state.t('deposit.qr_title'),
   orderTitle: state.t('deposit.order_title'),
   orderId: state.t('deposit.order_id'),
   gatewayCode: state.t('deposit.gateway_code'),
   amountLabel: state.t('deposit.amount_label'),
   diamondLabel: state.t('deposit.diamond_label'),
   bonusGoldLabel: state.t('deposit.bonus_gold_label'),
   errorTitle: state.t('deposit.error_title'),
   waitingWebhook: state.t('deposit.waiting_webhook'),
   failedReasonPrefix: state.t('deposit.failed_reason_prefix'),
  },
 };
}
