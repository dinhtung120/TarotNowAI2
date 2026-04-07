import { Coins, Gem } from "lucide-react";
import type { WalletBalance } from "@/features/wallet/domain/types";
import { Link } from "@/i18n/routing";

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
  <Link href="/wallet" className="inline-flex w-fit flex-col justify-center items-start gap-0 px-2 py-0.5 rounded-lg bg-[var(--bg-glass)] border border-[var(--border-subtle)] hover:bg-[var(--bg-glass-hover)] transition-all cursor-pointer group" title={title}>
   <div className="flex items-center gap-1 leading-none py-0.5" title={diamondTitle}>
    <Gem className="w-3 h-3 text-[var(--purple-accent)] group-hover:animate-pulse" />
    <span className="text-[var(--purple-muted)] font-bold text-[13px] tracking-tighter">{balance.diamondBalance.toLocaleString(locale)}</span>
    {balance.frozenDiamondBalance > 0 ? <span className="text-[var(--text-muted)] text-[10px] font-medium ml-0.5">{frozenSuffix(balance.frozenDiamondBalance.toLocaleString(locale))}</span> : null}
   </div>
   <div className="flex items-center gap-1 leading-none py-0.5 border-t border-[var(--border-subtle)] justify-start" title={goldTitle}>
    <Coins className="w-3 h-3 text-[color:var(--c-hex-b89c4f)] group-hover:animate-spin" style={{ animationDuration: "3s" }} />
    <span className="text-[color:var(--c-hex-b89c4f)] font-bold text-[13px] tracking-tighter">{balance.goldBalance.toLocaleString(locale)}</span>
   </div>
  </Link>
 );
}
