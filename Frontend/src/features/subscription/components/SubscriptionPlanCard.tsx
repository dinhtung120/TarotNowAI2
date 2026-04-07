"use client";

import { useTranslations } from "next-intl";
import { toast } from "react-hot-toast";
import { useWalletStore } from "@/store/walletStore";
import { useSubscribe } from "@/features/subscription/hooks/useSubscriptions";
import type { SubscriptionPlan } from "@/features/subscription/types";
import { SubscriptionEntitlementList } from "./SubscriptionEntitlementList";

interface SubscriptionPlanCardProps {
 plan: SubscriptionPlan;
}

export function SubscriptionPlanCard({ plan }: SubscriptionPlanCardProps) {
 const t = useTranslations("Subscription");
 const { mutate: subscribe, isPending } = useSubscribe();
 const walletBalance = useWalletStore((state) => state.balance?.diamondBalance || 0);

 const handleSubscribe = () => {
  if (walletBalance < plan.priceDiamond) return void toast.error(t("notEnoughDiamond"));
  const idempotencyKey = `sub_${plan.id}_${Date.now()}`;
  subscribe({ planId: plan.id, idempotencyKey }, { onSuccess: () => toast.success(t("subscribeSuccess", { name: plan.name })), onError: (error: Error) => toast.error(error.message || t("subscribeError")) });
 };

 return (
  <div className="bg-slate-800/50 border border-slate-700/50 rounded-2xl p-6 flex flex-col h-full hover:border-[#F6D365]/30 transition-colors">
   <div className="mb-4"><h3 className="text-xl font-bold text-white mb-2">{plan.name}</h3><p className="text-slate-400 text-sm">{plan.description}</p></div>
   <div className="mb-6 flex items-baseline gap-2"><span className="text-3xl font-extrabold text-[#F6D365]">{plan.priceDiamond}</span><span className="text-sm font-medium text-slate-400">/ {plan.durationDays} {t("days")}</span></div>
   <div className="flex-grow"><SubscriptionEntitlementList entitlements={plan.entitlements} unlimitedLabel={t("unlimited")} entitlementLabel={(key) => t(`entitlement_${key}`)} /></div>
   <button onClick={handleSubscribe} disabled={isPending} className="w-full mt-auto py-3 px-4 bg-gradient-to-r from-[#F6D365] to-[#FDA085] hover:opacity-90 disabled:opacity-50 text-slate-900 font-bold rounded-xl shadow-lg transition-all">{isPending ? t("processing") : t("buyNow")}</button>
  </div>
 );
}
