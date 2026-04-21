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
    bankInfoTitle: state.t("withdraw.bank_info_title"),
    bankNameLabel: state.t("withdraw.bank_label"),
    bankBinLabel: state.t("withdraw.bank_bin_label"),
    accountNameLabel: state.t("withdraw.account_name_label"),
    accountNumberLabel: state.t("withdraw.account_number_label"),
    bankInfoMissing: state.t("withdraw.bank_info_missing"),
    bankInfoUpdateCta: state.t("withdraw.bank_info_update_cta"),
    noteLabel: state.t("withdraw.note_label"),
    notePlaceholder: state.t("withdraw.note_placeholder"),
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
      userNotePrefix: state.t("withdraw.user_note_prefix"),
      emptyLabel: state.t("withdraw.history_empty"),
      title: state.t("withdraw.history_title"),
    },
  };
}
