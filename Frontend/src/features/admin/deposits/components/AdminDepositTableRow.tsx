import type { AdminDepositOrder } from '@/features/admin/deposits/actions/deposits';
import { cn } from "@/lib/utils";
import { AdminDepositAmountCell } from "./AdminDepositAmountCell";
import { AdminDepositAssetsCell } from "./AdminDepositAssetsCell";
import { AdminDepositRowActions } from "./AdminDepositRowActions";
import { AdminDepositStatusBadge } from "./AdminDepositStatusBadge";
import { AdminDepositTimeCell } from "./AdminDepositTimeCell";
import { AdminDepositUserCell } from "./AdminDepositUserCell";
import type { AdminDepositRowLabels } from "./types";

interface AdminDepositTableRowProps {
 locale: string;
 order: AdminDepositOrder;
 processingId: string | null;
 labels: AdminDepositRowLabels;
 onApprove: (order: AdminDepositOrder) => void;
 onReject: (order: AdminDepositOrder) => void;
}

export function AdminDepositTableRow({ locale, order, processingId, labels, onApprove, onReject }: AdminDepositTableRowProps) {
 return (
  <tr className={cn("group-row tn-hover-surface-strong transition-colors")}>
   <AdminDepositUserCell username={order.username} userId={order.userId} systemUserLabel={labels.systemUser} userIdPrefixLabel={labels.userIdPrefix} />
   <AdminDepositAmountCell locale={locale} amountVnd={order.amountVnd} />
   <AdminDepositAssetsCell locale={locale} diamondAmount={order.diamondAmount} />
   <AdminDepositTimeCell locale={locale} createdAt={order.createdAt} />
   <td className={cn("px-8 py-5 text-center")}><AdminDepositStatusBadge status={order.status} labels={{ success: labels.statusSuccess, failed: labels.statusFailed, pending: labels.statusPending }} /></td>
   <td className={cn("px-8 py-5 text-center")}><AdminDepositRowActions order={order} processingId={processingId} labels={{ approveTitle: labels.approveTitle, rejectTitle: labels.rejectTitle, notAvailable: labels.notAvailable }} onApprove={onApprove} onReject={onReject} /></td>
  </tr>
 );
}
