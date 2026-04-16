import { Coins, Gem } from "lucide-react";
import type { WalletBalance } from "@/features/wallet/domain/types";
import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
import { cn } from "@/lib/utils";

interface WalletWidgetContentProps {
 locale: string;
 title: string;
 diamondTitle: string;
 goldTitle: string;
 frozenSuffix: (amount: string) => string;
 balance: WalletBalance;
}

export function WalletWidgetContent({ locale, title, diamondTitle, goldTitle, frozenSuffix, balance }: WalletWidgetContentProps) {
 return (
  <Link
   href="/wallet"
   className={cn(
    "group",
    "inline-flex",
    "w-fit",
    "cursor-pointer",
    "flex-col",
    "items-start",
   "justify-center",
   "gap-0",
   "rounded-lg",
   "border",
    "tn-wallet-widget-shell",
    "px-2",
    "py-0.5",
    "transition-all",
   )}
   title={title}
  >
   <div className={cn("flex", "items-center", "gap-1", "py-0.5", "leading-none")} title={diamondTitle}>
    <Gem className={cn("h-3", "w-3", "text-violet-400")} />
    <span className={cn("tn-text-13", "font-bold", "tracking-tighter", "text-violet-300")}>{balance.diamondBalance.toLocaleString(locale)}</span>
    {balance.frozenDiamondBalance > 0 ? <span className={cn("ml-0.5", "text-xs", "font-medium", "text-slate-400")}>{frozenSuffix(balance.frozenDiamondBalance.toLocaleString(locale))}</span> : null}
   </div>
   <div className={cn("flex", "items-center", "justify-start", "gap-1", "border-t", "border-slate-700", "py-0.5", "leading-none")} title={goldTitle}>
    <Coins className={cn("h-3", "w-3", "text-amber-500")} style={{ animationDuration: "3s" }} />
    <span className={cn("tn-text-13", "font-bold", "tracking-tighter", "text-amber-500")}>{balance.goldBalance.toLocaleString(locale)}</span>
   </div>
  </Link>
 );
}
