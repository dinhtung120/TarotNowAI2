import { cn } from "@/lib/utils";

interface AdminDepositsTableHeadProps {
 labels: {
  headingUser: string;
  headingAmount: string;
  headingAssets: string;
  headingTime: string;
  headingStatus: string;
  headingActions: string;
 };
}

export function AdminDepositsTableHead({ labels }: AdminDepositsTableHeadProps) {
 return (
  <thead>
   <tr className={cn("border-b tn-border-soft tn-surface")}>
    <th className={cn("px-8 py-6 tn-text-overline tn-text-tertiary")}>{labels.headingUser}</th>
    <th className={cn("px-8 py-6 tn-text-overline tn-text-tertiary")}>{labels.headingAmount}</th>
    <th className={cn("px-8 py-6 tn-text-overline tn-text-tertiary")}>{labels.headingAssets}</th>
    <th className={cn("px-8 py-6 tn-text-overline tn-text-tertiary")}>{labels.headingTime}</th>
    <th className={cn("px-8 py-6 tn-text-overline tn-text-tertiary text-center")}>{labels.headingStatus}</th>
    <th className={cn("px-8 py-6 tn-text-overline tn-text-tertiary text-center")}>{labels.headingActions}</th>
   </tr>
  </thead>
 );
}
