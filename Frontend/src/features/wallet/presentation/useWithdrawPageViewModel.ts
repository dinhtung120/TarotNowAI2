import type { WithdrawFormCardLabels } from "@/features/wallet/presentation/components/withdraw/WithdrawFormCard.types";
import { useWithdrawPage } from "@/features/wallet/application/useWithdrawPage";

export function useWithdrawPageViewModel() {
  const state = useWithdrawPage();

  const formLabels: WithdrawFormCardLabels = {
    amountLabel: state.t("withdraw.amount_label"),
    amountPlaceholder: state.t("withdraw.amount_placeholder"),
    grossLabel: state.t("withdraw.gross_label"),
    feeLabel: state.t("withdraw.fee_label"),
    netLabel: state.t("withdraw.net_label"),
    bankLabel: state.t("withdraw.bank_label"),
    bankPlaceholder: state.t("withdraw.bank_placeholder"),
    accountNameLabel: state.t("withdraw.account_name_label"),
    accountNamePlaceholder: state.t("withdraw.account_name_placeholder"),
    accountNumberLabel: state.t("withdraw.account_number_label"),
    accountNumberPlaceholder: state.t("withdraw.account_number_placeholder"),
    successMessage: state.t("withdraw.success_message"),
    submittingLabel: state.t("withdraw.submitting"),
    submitLabel: state.t("withdraw.submit"),
  };

  return {
    ...state,
    formLabels,
    headerLabels: {
      backLabel: state.t("withdraw.back_to_wallet"),
      tag: state.t("withdraw.tag"),
      title: state.t("withdraw.title"),
      subtitle: state.t("withdraw.subtitle"),
    },
    historyLabels: {
      adminNotePrefix: state.t("withdraw.admin_note_prefix"),
      emptyLabel: state.t("withdraw.history_empty"),
      title: state.t("withdraw.history_title"),
    },
  };
}
