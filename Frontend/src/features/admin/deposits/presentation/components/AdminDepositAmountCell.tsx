import { cn } from "@/lib/utils";
import { formatCurrency } from "@/shared/utils/format/formatCurrency";

interface AdminDepositAmountCellProps {
 locale: string;
 amountVnd: number;
}

export function AdminDepositAmountCell({ locale, amountVnd }: AdminDepositAmountCellProps) {
 return (
  <td className={cn("px-8 py-5")}>
   <div className={cn("tn-text-11 font-black tn-text-primary uppercase tracking-tighter")}>
    {formatCurrency(amountVnd, locale)}
   </div>
  </td>
 );
}
