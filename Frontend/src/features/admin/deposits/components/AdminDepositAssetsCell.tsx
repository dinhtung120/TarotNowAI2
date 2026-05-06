import { Gem } from "lucide-react";
import { cn } from "@/lib/utils";

interface AdminDepositAssetsCellProps {
 locale: string;
 diamondAmount: number;
}

export function AdminDepositAssetsCell({ locale, diamondAmount }: AdminDepositAssetsCellProps) {
 return (
  <td className={cn("px-8 py-5")}>
   <div className={cn("flex items-center gap-2 tn-text-11 font-black tn-text-accent italic")}>
    <Gem className={cn("w-3.5 h-3.5")} />
    +{diamondAmount.toLocaleString(locale)}
   </div>
  </td>
 );
}
