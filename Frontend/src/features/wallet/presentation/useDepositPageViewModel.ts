import { formatCurrency } from "@/shared/utils/format/formatCurrency";
import { useDepositPage } from "@/features/wallet/application/useDepositPage";

export function useDepositPageViewModel() {
  const state = useDepositPage();
  const formatVnd = (value: number) => formatCurrency(value, state.locale);

  return {
    ...state,
    formatVnd,
    headerLabels: {
      backLabel: state.t("deposit.back_to_wallet"),
      tag: state.t("deposit.tag"),
      title: state.t("deposit.title"),
      subtitle: state.t("deposit.subtitle"),
    },
    leftColumnLabels: {
      balanceLabel: state.t("deposit.balance_label"),
      selectionTitle: state.t("deposit.select_title"),
      customLabel: state.t("deposit.custom_label"),
      customPlaceholder: state.t("deposit.custom_placeholder"),
      bonusLabel: (amount: number) => state.t("deposit.bonus_gold", { amount }),
    },
    rightColumnLabels: {
      notesTitle: state.t("deposit.notes_title"),
      notesItems: [
        state.t("deposit.notes_item1"),
        state.t("deposit.notes_item2"),
        state.t("deposit.notes_item3"),
      ] as [string, string, string],
      summary: {
        title: state.t("deposit.confirm_title"),
        submitting: state.t("deposit.submitting"),
        submit: state.t("deposit.submit"),
        securityNote: state.t("deposit.security_note"),
        valueLabel: state.t("deposit.value_label"),
        diamondReceiveLabel: state.t("deposit.diamond_receive_label"),
        promoBonusLabel: state.t("deposit.promo_bonus_label"),
        totalAssetsLabel: state.t("deposit.total_assets_label"),
        orderReady: state.t("deposit.order_ready"),
        payNow: state.t("deposit.pay_now"),
      },
    },
    onCustomFocus: () => {
      state.setIsCustom(true);
      state.resetOrderState();
    },
    onCustomChange: (value: string) => {
      state.setCustomAmount(value);
      state.setIsCustom(true);
      state.resetOrderState();
    },
  };
}
