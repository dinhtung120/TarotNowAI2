"use client";

import { AlertTriangle } from "lucide-react";
import type { DepositPromotion } from "@/features/admin/application/actions/promotion";
import { ActionConfirmModal } from "@/shared/components/ui";
import { useAdminPromotions } from "@/features/admin/promotions/application/useAdminPromotions";
import { AdminPromotionCreateForm, AdminPromotionsHeader, AdminPromotionsList } from "./components";
import { cn } from "@/lib/utils";

interface AdminPromotionsClientProps {
 initialPromotions: DepositPromotion[];
 initialLoadError: string | null;
}

export default function AdminPromotionsClient({ initialPromotions, initialLoadError }: AdminPromotionsClientProps) {
 const vm = useAdminPromotions(initialPromotions, initialLoadError);

 return (
  <div className={cn("space-y-8", "pb-20", "animate-in", "fade-in", "duration-700")}>
   <AdminPromotionsHeader
    isCreating={vm.isCreating}
    onToggleCreate={() => vm.setIsCreating((prev) => !prev)}
   />
   {vm.isCreating ? (
    <AdminPromotionCreateForm
     minAmount={vm.minAmount}
     bonusGold={vm.bonusGold}
     submitting={vm.submitting}
     onMinAmountChange={vm.setMinAmount}
     onBonusGoldChange={vm.setBonusGold}
     onSubmit={vm.handleCreate}
    />
   ) : null}
   <AdminPromotionsList
   promotions={vm.promotions}
    loading={vm.loading}
    listError={vm.listError}
    onRetry={vm.retryList}
    locale={vm.locale}
    formatMoney={vm.formatMoney}
    togglingId={vm.togglingId}
    onToggle={vm.handleToggle}
    onDelete={vm.setDeleteId}
   />
   <ActionConfirmModal
    open={Boolean(vm.deleteId)}
    onCancel={() => vm.setDeleteId(null)}
    onConfirm={vm.handleDelete}
    icon={<div className={cn("mx-auto", "flex", "h-16", "w-16", "items-center", "justify-center", "rounded-2xl", "border", "border-red-500/20", "bg-red-500/10", "text-red-400", "shadow-inner")}><AlertTriangle className={cn("h-8", "w-8")} /></div>}
    title={vm.t("promotions.delete_modal.title")}
    description={vm.t("promotions.delete_modal.desc")}
    cancelLabel={vm.t("promotions.delete_modal.keep")}
    confirmLabel={vm.t("promotions.delete_modal.delete")}
    confirmVariant="danger"
    confirmDisabled={Boolean(vm.deletingId)}
   />
  </div>
 );
}
