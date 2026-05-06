import { CreditCard, Loader2 } from "lucide-react";
import type { AdminDepositOrder } from '@/features/admin/deposits/actions/deposits';
import { TableStates } from "@/shared/ui";
import { cn } from "@/lib/utils";
import { AdminDepositTableRow } from "./AdminDepositTableRow";
import type { AdminDepositRowLabels } from "./types";

interface AdminDepositsTableBodyProps {
 locale: string;
 orders: AdminDepositOrder[];
 loading: boolean;
 errorLabel: string;
 processingId: string | null;
 labels: AdminDepositRowLabels & { loading: string; empty: string };
 onApprove: (order: AdminDepositOrder) => void;
 onReject: (order: AdminDepositOrder) => void;
}

export function AdminDepositsTableBody({ locale, orders, loading, errorLabel, processingId, labels, onApprove, onReject }: AdminDepositsTableBodyProps) {
 const hasError = !loading && errorLabel.trim().length > 0;
 return (
  <tbody className={cn("divide-y divide-white/5")}>
   <TableStates
    colSpan={6}
   isLoading={loading}
   isError={hasError}
   errorLabel={errorLabel}
   isEmpty={!loading && !hasError && orders.length === 0}
   loadingLabel={labels.loading}
   emptyLabel={labels.empty}
    loadingIcon={<Loader2 className={cn("w-8 h-8 animate-spin tn-text-accent")} />}
    emptyIcon={<div className={cn("w-16 h-16 rounded-full tn-panel-soft flex items-center justify-center")}><CreditCard className={cn("w-8 h-8 tn-text-tertiary opacity-50")} /></div>}
   />
   {loading || hasError ? null : orders.map((order) => (
    <AdminDepositTableRow
     key={order.id}
     locale={locale}
     order={order}
     processingId={processingId}
     labels={labels}
     onApprove={onApprove}
     onReject={onReject}
    />
   ))}
  </tbody>
 );
}
