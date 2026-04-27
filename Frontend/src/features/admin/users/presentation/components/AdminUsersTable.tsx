import { Loader2, Users } from "lucide-react";
import { GlassCard, StepPagination, TableStates } from "@/shared/components/ui";
import { cn } from "@/lib/utils";
import type { AdminUsersTableProps } from "./types";
import { AdminUserTableRow } from "./AdminUserTableRow";

export function AdminUsersTable({ errorLabel, loading, locale, onEdit, onNextPage, onPrevPage, page, t, totalCount, users }: AdminUsersTableProps) {
 const hasError = Boolean(errorLabel);
 return (
  <GlassCard className={cn("!p-0 !tn-rounded-2_5xl overflow-hidden")}>
   <div className={cn("overflow-x-auto custom-scrollbar")}>
    <table className={cn("w-full text-left")}>
     <thead>
      <tr className={cn("border-b tn-border-soft tn-surface")}>
       <th className={cn("px-8 py-6 tn-text-overline tn-text-tertiary")}>{t("users.table.heading_account")}</th>
       <th className={cn("px-8 py-6 tn-text-overline tn-text-tertiary")}>{t("users.table.heading_rank")}</th>
       <th className={cn("px-8 py-6 tn-text-overline tn-text-tertiary")}>{t("users.table.heading_assets")}</th>
       <th className={cn("px-8 py-6 tn-text-overline tn-text-tertiary")}>{t("users.table.heading_role")}</th>
       <th className={cn("px-8 py-6 tn-text-overline tn-text-tertiary text-center")}>{t("users.table.heading_status")}</th>
       <th className={cn("px-8 py-6 tn-text-overline tn-text-tertiary text-right")}>{t("users.table.heading_actions")}</th>
      </tr>
     </thead>
     <tbody className={cn("divide-y divide-white/5")}>
      <TableStates colSpan={6} isLoading={loading} isError={hasError} errorLabel={errorLabel || t("users.toast.system_error")} isEmpty={!loading && !hasError && users.length === 0} loadingLabel={t("users.states.loading")} emptyLabel={t("users.states.empty")} loadingIcon={<Loader2 className={cn("w-8 h-8 animate-spin tn-text-accent")} />} emptyIcon={<div className={cn("w-16 h-16 rounded-full tn-panel-soft flex items-center justify-center")}><Users className={cn("w-8 h-8 tn-text-tertiary opacity-50")} /></div>} />
      {loading || hasError ? null : users.map((user) => <AdminUserTableRow key={user.id} user={user} locale={locale} onEdit={onEdit} t={t} />)}
     </tbody>
    </table>
   </div>
   <StepPagination summary={t("users.pagination.summary", { page, total: totalCount })} currentLabel={String(page)} canPrev={page > 1} canNext={page * 10 < totalCount} onPrev={onPrevPage} onNext={onNextPage} />
  </GlassCard>
 );
}
