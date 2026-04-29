import { GlassCard, StepPagination } from "@/shared/components/ui";
import { cn } from "@/lib/utils";
import { AdminDepositsTableBody } from "./AdminDepositsTableBody";
import { AdminDepositsTableHead } from "./AdminDepositsTableHead";
import type { AdminDepositsTableProps } from "./types";

export function AdminDepositsTable({ locale, orders, loading, errorLabel, page, totalCount, processingId, labels, onApprove, onReject, onPrev, onNext }: AdminDepositsTableProps) {
 return (
  <GlassCard className={cn("!p-0 !tn-rounded-2_5xl overflow-hidden text-left")}>
   <div className={cn("overflow-x-auto custom-scrollbar")}>
    <table className={cn("w-full text-left")}>
     <AdminDepositsTableHead labels={labels} />
     <AdminDepositsTableBody locale={locale} orders={orders} loading={loading} errorLabel={errorLabel} processingId={processingId} labels={labels} onApprove={onApprove} onReject={onReject} />
    </table>
   </div>
   <StepPagination summary={labels.summary} currentLabel={String(page)} canPrev={page > 1} canNext={page * 10 < totalCount} onPrev={onPrev} onNext={onNext} />
  </GlassCard>
 );
}
