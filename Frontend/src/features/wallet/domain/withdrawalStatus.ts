export interface WithdrawalStatusBadge {
 text: string;
 className: string;
}

export interface WithdrawalStatusLabels {
 pending: string;
 approved: string;
 rejected: string;
 paid: string;
}

export function getWithdrawalStatusBadge(
 status: string,
 labels: WithdrawalStatusLabels
): WithdrawalStatusBadge {
 switch (status) {
  case 'pending':
   return {
    text: labels.pending,
    className: 'bg-[var(--warning)]/10 text-[var(--warning)] border-[var(--warning)]/20',
   };
  case 'approved':
   return {
    text: labels.approved,
    className: 'bg-[var(--success)]/10 text-[var(--success)] border-[var(--success)]/20',
   };
  case 'rejected':
   return {
    text: labels.rejected,
    className: 'bg-[var(--danger)]/10 text-[var(--danger)] border-[var(--danger)]/20',
   };
  case 'paid':
   return {
    text: labels.paid,
    className: 'bg-[var(--info)]/10 text-[var(--info)] border-[var(--info)]/20',
   };
  default:
   return {
    text: status,
    className: 'bg-[var(--bg-surface-hover)] text-[var(--text-secondary)] border-[var(--border-subtle)]',
   };
 }
}
