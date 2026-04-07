import { User } from "lucide-react";
import { cn } from "@/lib/utils";

interface AdminDepositUserCellProps {
 username?: string;
 userId: string;
 systemUserLabel: string;
 userIdPrefixLabel: (id: string) => string;
}

export function AdminDepositUserCell({ username, userId, systemUserLabel, userIdPrefixLabel }: AdminDepositUserCellProps) {
 return (
  <td className={cn("px-8 py-5")}>
   <div className={cn("flex items-center gap-3")}>
    <div className={cn("w-8 h-8 rounded-lg tn-panel-overlay flex items-center justify-center shadow-inner")}>
     <User className={cn("w-4 h-4 text-[var(--text-secondary)]")} />
    </div>
    <div>
     <div className={cn("text-[11px] font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm")}>{username || systemUserLabel}</div>
     <div className={cn("text-[9px] font-bold text-[var(--text-tertiary)] italic tracking-tighter")}>{userIdPrefixLabel(userId.split("-")[0])}</div>
    </div>
   </div>
  </td>
 );
}
