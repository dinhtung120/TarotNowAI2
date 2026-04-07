"use client";

import { useTranslations } from "next-intl";
import { toast } from "react-hot-toast";
import { cn } from "@/lib/utils";
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
  <div
   className={cn(
    "flex",
    "h-full",
    "flex-col",
    "rounded-2xl",
    "border",
    "border-slate-700/50",
    "bg-slate-800/50",
    "p-6",
    "transition-colors",
   )}
  >
   <div className={cn("mb-4")}>
    <h3 className={cn("mb-2", "text-xl", "font-bold", "text-white")}>{plan.name}</h3>
    <p className={cn("text-sm", "text-slate-400")}>{plan.description}</p>
   </div>
   <div className={cn("mb-6", "flex", "items-baseline", "gap-2")}>
    <span className={cn("text-3xl", "font-extrabold", "text-amber-300")}>{plan.priceDiamond}</span>
    <span className={cn("text-sm", "font-medium", "text-slate-400")}>
     / {plan.durationDays} {t("days")}
    </span>
   </div>
   <div className={cn("flex-grow")}>
    <SubscriptionEntitlementList
     entitlements={plan.entitlements}
     unlimitedLabel={t("unlimited")}
     entitlementLabel={(key) => t(`entitlement_${key}`)}
    />
   </div>
   <button
    type="button"
    onClick={handleSubscribe}
    disabled={isPending}
    className={cn(
     "mt-auto",
     "w-full",
     "rounded-xl",
     "bg-gradient-to-r",
     "from-amber-300",
     "to-orange-300",
     "px-4",
     "py-3",
     "font-bold",
     "text-slate-900",
     "shadow-lg",
     "transition-all",
     "disabled:opacity-50",
    )}
   >
    {isPending ? t("processing") : t("buyNow")}
   </button>
  </div>
 );
}
