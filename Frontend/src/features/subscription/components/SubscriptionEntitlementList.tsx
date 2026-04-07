import type { SubscriptionPlan } from "@/features/subscription/types";

interface SubscriptionEntitlementListProps {
 entitlements: SubscriptionPlan["entitlements"];
 unlimitedLabel: string;
 entitlementLabel: (key: string) => string;
}

export function SubscriptionEntitlementList({ entitlements, unlimitedLabel, entitlementLabel }: SubscriptionEntitlementListProps) {
 return (
  <ul className="space-y-3 mb-6">
   {Object.entries(entitlements).map(([key, quota]) => (
    <li key={key} className="flex items-start text-sm text-slate-300">
     <svg className="w-5 h-5 text-emerald-400 mr-3 shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" /></svg>
     {quota > 0 ? <span>{quota} <span className="text-slate-400 ml-1">{entitlementLabel(key)}</span></span> : <span>{unlimitedLabel} <span className="text-slate-400 ml-1">{entitlementLabel(key)}</span></span>}
    </li>
   ))}
  </ul>
 );
}
