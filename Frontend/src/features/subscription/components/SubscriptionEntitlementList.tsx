import type { SubscriptionPlan } from "@/features/subscription/types";
import { cn } from "@/lib/utils";

interface SubscriptionEntitlementListProps {
 entitlements: SubscriptionPlan["entitlements"];
 unlimitedLabel: string;
 entitlementLabel: (key: string) => string;
}

export function SubscriptionEntitlementList({ entitlements, unlimitedLabel, entitlementLabel }: SubscriptionEntitlementListProps) {
 return (
  <ul className={cn("mb-6", "space-y-3")}>
   {Object.entries(entitlements).map(([key, quota]) => (
    <li key={key} className={cn("flex", "items-start", "text-sm", "text-slate-300")}>
     <svg className={cn("mr-3", "h-5", "w-5", "shrink-0", "text-emerald-400")} fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" /></svg>
     {quota > 0 ? <span>{quota} <span className={cn("ml-1", "text-slate-400")}>{entitlementLabel(key)}</span></span> : <span>{unlimitedLabel} <span className={cn("ml-1", "text-slate-400")}>{entitlementLabel(key)}</span></span>}
    </li>
   ))}
  </ul>
 );
}
