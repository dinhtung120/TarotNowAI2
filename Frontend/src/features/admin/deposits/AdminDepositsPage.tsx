"use client";

import { ThumbsDown, ThumbsUp } from "lucide-react";
import { ActionConfirmModal } from "@/shared/ui";
import { useAdminDeposits } from "@/features/admin/deposits/hooks/useAdminDeposits";
import { AdminDepositsHeader, AdminDepositsSummary, AdminDepositsTable } from "./components";
import { getAdminDepositsPageLabels } from "./useAdminDepositsPageLabels";
import { cn } from "@/lib/utils";

export default function AdminDepositsPage() {
 const vm = useAdminDeposits();
 const labels = getAdminDepositsPageLabels(vm.t, vm.page, vm.totalCount, vm.confirmModal.type);
 const modalIcon = vm.confirmModal.type === "approve"
  ? <ThumbsUp className={cn("h-8", "w-8")} />
  : <ThumbsDown className={cn("h-8", "w-8")} />;
 const modalIconClassName = vm.confirmModal.type === "approve"
  ? "bg-emerald-500/10 border-emerald-500/20 text-emerald-400"
  : "bg-red-500/10 border-red-500/20 text-red-400";

 return (
  <div className={cn("space-y-8", "pb-20", "animate-in", "fade-in", "duration-700")}>
   <ActionConfirmModal
    open={vm.confirmModal.isOpen}
    onCancel={() => vm.setConfirmModal((prev) => ({ ...prev, isOpen: false }))}
    onConfirm={vm.handleAction}
    icon={<div className={cn("flex", "h-16", "w-16", "items-center", "justify-center", "rounded-2xl", "border", "shadow-inner", modalIconClassName)}>{modalIcon}</div>}
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
    errorLabel={vm.listError}
    page={vm.page}
    totalCount={vm.totalCount}
    processingId={vm.processingId}
    labels={labels.table}
    onApprove={(order) => vm.setConfirmModal({ isOpen: true, type: "approve", order })}
    onReject={(order) => vm.setConfirmModal({ isOpen: true, type: "reject", order })}
    onPrev={() => vm.setPage((currentPage) => Math.max(1, currentPage - 1))}
    onNext={() => vm.setPage((currentPage) => currentPage + 1)}
   />
   {!vm.loading && !vm.listError && vm.orders.length > 0 ? <AdminDepositsSummary labels={labels.summary} /> : null}
  </div>
 );
}
