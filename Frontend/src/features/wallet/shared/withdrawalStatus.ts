interface WithdrawalStatusBadge {
 text: string;
 className: string;
}

interface WithdrawalStatusLabels {
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
    className: 'tn-bg-warning-10 tn-text-warning tn-border-warning-20',
   };
  case 'approved':
   return {
    text: labels.approved,
    className: 'tn-bg-success-10 tn-text-success tn-border-success-20',
   };
  case 'rejected':
   return {
    text: labels.rejected,
    className: 'tn-bg-danger-soft tn-text-danger tn-border-danger',
   };
  case 'paid':
   return {
    text: labels.paid,
    className: 'tn-bg-info-10 tn-text-info tn-border-info-20',
   };
  default:
   return {
    text: status,
    className: 'tn-bg-surface-hover tn-text-secondary tn-border-soft',
   };
 }
}
