"use client";

import { ThumbsDown, ThumbsUp } from "lucide-react";
import { ActionConfirmModal } from "@/shared/components/ui";
import { useAdminDeposits } from "@/features/admin/deposits/application/useAdminDeposits";
import { AdminDepositsHeader, AdminDepositsSummary, AdminDepositsTable } from "./components";
import { getAdminDepositsPageLabels } from "./useAdminDepositsPageLabels";

export default function AdminDepositsPage() {
 const vm = useAdminDeposits();
 const labels = getAdminDepositsPageLabels(vm.t, vm.page, vm.totalCount, vm.confirmModal.type);
 const modalIcon = vm.confirmModal.type === "approve"
  ? <ThumbsUp className="w-8 h-8" />
  : <ThumbsDown className="w-8 h-8" />;
 const modalIconClassName = vm.confirmModal.type === "approve"
  ? "bg-[var(--success)]/10 border-[var(--success)]/20 text-[var(--success)]"
  : "bg-[var(--danger)]/10 border-[var(--danger)]/20 text-[var(--danger)]";

 return (
  <div className="space-y-8 pb-20 animate-in fade-in duration-700">
   <ActionConfirmModal
    open={vm.confirmModal.isOpen}
    onCancel={() => vm.setConfirmModal((prev) => ({ ...prev, isOpen: false }))}
    onConfirm={vm.handleAction}
    icon={<div className={`w-16 h-16 rounded-2xl flex items-center justify-center border shadow-inner ${modalIconClassName}`}>{modalIcon}</div>}
    title={labels.modal.title}
    description={labels.modal.description}
    cancelLabel={labels.modal.cancelLabel}
    confirmLabel={labels.modal.confirmLabel}
    confirmVariant={labels.modal.confirmVariant}
   />
   <AdminDepositsHeader
    statusFilter={vm.statusFilter}
    labels={labels.header}
    onStatusFilterChange={(value) => {
     vm.setStatusFilter(value);
     vm.setPage(1);
    }}
   />
   <AdminDepositsTable
    locale={vm.locale}
    orders={vm.orders}
    loading={vm.loading}
    page={vm.page}
    totalCount={vm.totalCount}
    processingId={vm.processingId}
    labels={labels.table}
    onApprove={(order) => vm.setConfirmModal({ isOpen: true, type: "approve", order })}
    onReject={(order) => vm.setConfirmModal({ isOpen: true, type: "reject", order })}
    onPrev={() => vm.setPage((currentPage) => Math.max(1, currentPage - 1))}
    onNext={() => vm.setPage((currentPage) => currentPage + 1)}
   />
   {!vm.loading && vm.orders.length > 0 ? <AdminDepositsSummary labels={labels.summary} /> : null}
  </div>
 );
}
