import type { AdminDepositsTableLabels } from "./components/types";
import type { useAdminDeposits } from "@/features/admin/deposits/application/useAdminDeposits";

type Translator = ReturnType<typeof useAdminDeposits>["t"];

type AdminDepositsModalLabels = {
 title: string;
 description: string;
 cancelLabel: string;
 confirmLabel: string;
 confirmVariant: "primary" | "danger";
};

export function getAdminDepositsPageLabels(t: Translator, page: number, totalCount: number, modalType: "approve" | "reject") {
 const header = {
  tag: t("deposits.header.tag"),
  title: t("deposits.header.title"),
  subtitle: t("deposits.header.subtitle", { count: totalCount }),
  filterLabel: t("deposits.filters.label"),
  all: t("deposits.filters.all"),
  pending: t("deposits.filters.pending"),
  success: t("deposits.filters.success"),
  failed: t("deposits.filters.failed"),
 };

 const table: AdminDepositsTableLabels = {
  headingUser: t("deposits.table.heading_user"),
  headingAmount: t("deposits.table.heading_amount"),
  headingAssets: t("deposits.table.heading_assets"),
  headingTime: t("deposits.table.heading_time"),
  headingStatus: t("deposits.table.heading_status"),
  headingActions: t("deposits.table.heading_actions"),
  loading: t("deposits.states.loading"),
  empty: t("deposits.states.empty"),
  systemUser: t("deposits.row.system_user"),
  userIdPrefix: (id) => t("deposits.row.user_id_prefix", { id }),
  statusSuccess: t("deposits.status.success"),
  statusFailed: t("deposits.status.failed"),
  statusPending: t("deposits.status.pending"),
  approveTitle: t("deposits.actions.approve_title"),
  rejectTitle: t("deposits.actions.reject_title"),
  notAvailable: t("deposits.actions.na"),
  summary: t("deposits.pagination.summary", { page, total: totalCount }),
 };

 const summary = {
  confirmTitle: t("deposits.summary.confirm_title"),
  confirmDescription: t("deposits.summary.confirm_desc"),
  ledgerTitle: t("deposits.summary.ledger_title"),
  ledgerSubtitle: t("deposits.summary.ledger_subtitle"),
 };

 const modal: AdminDepositsModalLabels = {
  title: modalType === "approve" ? t("deposits.modal.title_approve") : t("deposits.modal.title_reject"),
  description: modalType === "approve" ? t("deposits.modal.desc_approve") : t("deposits.modal.desc_reject"),
  cancelLabel: t("deposits.modal.back"),
  confirmLabel: t("deposits.modal.confirm"),
  confirmVariant: modalType === "approve" ? "primary" : "danger",
 };

 return { header, table, summary, modal };
}
